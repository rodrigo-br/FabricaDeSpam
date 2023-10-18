namespace WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.IO;
    using System.Text;
    using WebApp.Models;
    using Microsoft.AspNetCore.StaticFiles;
    using System.Text.Json;
    using WebApp.Services.Interfaces;
    using WebApp.Services;

    public class ImageController : Controller
    {
        private readonly string[] allowedExtensions;
        private readonly string producerBaseUrl;
        private readonly string imageBaseUrl;
        private readonly IUserService _userService;

        public ImageController(IUserService userService)
        {

            allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
            producerBaseUrl = "http://webapi:80/api/Producer/";
            imageBaseUrl = "http://webapi:80/api/Image/";
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Sender()
        {
            if (AuthenticationUtility.IsAuthenticated(HttpContext) == null)
            {
                return RedirectToAction("Login", "Account");
            }
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

            HttpResponseMessage? idResponse = await _userService
                .GetUserId(HttpContext.Session.GetString("AuthToken"));
            if (idResponse != null)
            {
                imageViewModel.UserId = await idResponse.Content.ReadAsStringAsync();
            }
            else
            {
                var _returnUrl = Url.Action("Sender", "Image");
                return RedirectToAction("Login", "Account", new { returnUrl = _returnUrl});
            }

            using (var httpClient = new HttpClient())
            {
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
            if (AuthenticationUtility.IsAuthenticated(HttpContext) == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
		}

        [HttpPost]
        public IActionResult Receiver(List<string> topics)
        {
            foreach (var topic in topics)
            {
                Console.WriteLine(topic);
            }
            return RedirectToAction("Index", "Home");
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
                    if (images != null)
                    {
                        images.Reverse();
					    return View(images);
                    }
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
