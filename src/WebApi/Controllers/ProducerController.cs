namespace WebApi.Controllers
{
    using Domain.DTO;
    using Domain.Entities;
    using Infrastructure.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Producer.Interface;

    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly string directory;
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly IImageRepository _imageRepository;
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public ProducerController(IKafkaProducerService kafkaProducer, IImageRepository imageRepository)
        {
            _kafkaProducer = kafkaProducer;
            _imageRepository = imageRepository;
            directory = Path.Combine(Directory.GetCurrentDirectory(), "files");
        }

        //[HttpGet]
        //public async Task<IEnumerable<ImageDTO>> GetAll()
        //{
        //    return await _imageRepository.GetAll();
        //}

        [HttpGet]
        public async Task<ImageDTO?> GetById(Guid id)
        {
            return await _imageRepository.GetById(id);
        }

        [HttpPost]
        [Route("cat")]
        public async Task<IActionResult> UploadFile(IFormFile file)
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

            string randomName = SetRandomName(file.FileName);
            string fullPath = Path.Combine(directory, randomName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }

            bool success = await _kafkaProducer.ProduceMessageAsync("cat", randomName, fileData);
            if (success)
            {
                return Ok(new { mensagem = "Imagem salva com sucesso!" });
            }
            else
            {
                return StatusCode(500, "Ocorreu algum erro no envio da mensagem");
            }
        }

        private static string SetRandomName(string originalName)
        {
            string baseName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            string extension = Path.GetExtension(originalName);
            return $"{baseName}{extension}";
        }
    }
}
