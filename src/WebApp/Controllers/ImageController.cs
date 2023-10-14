namespace WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.IO;
    using System.Text;
    using WebApp.Models;
    using Microsoft.AspNetCore.StaticFiles;
    using System.Text.Json;

    public class ImageController : Controller
    {
        private readonly string[] allowedExtensions;
        private readonly string producerBaseUrl;
        private readonly string userBaseUrl;
        private readonly string imageBaseUrl;

        public ImageController()
        {
            allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
            producerBaseUrl = "http://webapi:80/api/Producer/";
            userBaseUrl = "http://webapi:80/api/User/";
            imageBaseUrl = "http://webapi:80/api/Image/";
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Sender()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Sender(IFormFile file, List<string> topics)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhuma imagem enviada.");
            }
            if (file.Length > 5000000)
            {
                throw new BadHttpRequestException("A imagem não pode ser maior que 5MB.", 413);
            }

            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return StatusCode(415, "Tipo de mídia não suportado. Apenas imagens com as seguintes extensões são permitidas: " + string.Join(", ", allowedExtensions));
            }

            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }
            ImageViewModel imageViewModel = new ImageViewModel
            {
                ImageData = fileData,
                Topics = topics,
                FileName = SetRandomName(file.FileName),
                UserId = String.Empty
            };
            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(fileExtension, out var contentType);
            imageViewModel.MimeType = contentType ?? String.Empty;
            using (var httpClient = new HttpClient())
            {
                string? token = HttpContext.Session.GetString("AuthToken");
                if (token == null)
                {
                    return BadRequest("Necessary login first");
                }

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage idResponse = await httpClient.GetAsync(userBaseUrl + "IdClaimer");
                if (idResponse.IsSuccessStatusCode)
                {
                    imageViewModel.UserId = await idResponse.Content.ReadAsStringAsync();
                }
                else
                {
                    return Unauthorized("Unauthorized");
                }
                Console.WriteLine($"User id: {imageViewModel.UserId}");
                string jsonImageViewModel = JsonConvert.SerializeObject(imageViewModel);
                var content = new StringContent(jsonImageViewModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(producerBaseUrl + "send", content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Image sent");
                }
            }
            return BadRequest();
        }

		[HttpGet]
		public IActionResult Receiver()
		{
            return View();
		}

        [HttpGet]
        public async Task<IActionResult> ImageCollection()
        {
			using (var httpClient = new HttpClient())
			{
				HttpResponseMessage response = await httpClient.GetAsync(imageBaseUrl + "AllImages");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
                    var images = JsonConvert.DeserializeObject<List<ImageViewModel>>(content);
					return View(images);
				}
			}
			throw new Exception();
        }

        private static string SetRandomName(string originalName)
        {
            string baseName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            string extension = Path.GetExtension(originalName);
            return $"{baseName}{extension}";
        }
    }
}
