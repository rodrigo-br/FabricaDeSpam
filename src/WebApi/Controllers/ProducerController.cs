namespace WebApi.Controllers
{
    using Infrastructure.ApplicationContext;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Producer.Interface;

    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly string directory;
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly ApplicationDbContext _dbContext;

        public ProducerController(IKafkaProducerService kafkaProducer, ApplicationDbContext dbContext)
        {
            _kafkaProducer = kafkaProducer;
            _dbContext = dbContext;
            directory = Path.Combine(Directory.GetCurrentDirectory(), "files");
        }

        [HttpGet]
        [Route("test")]
        public async Task<string?> TestConnection()
        {
            var test = await _dbContext.Images.ToListAsync();

            if (test != null)
            {
                return test.ToString();
            }
            return "fail";
        }

        [HttpPost]
        [Route("cat")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file.Length > 5000000)
            {
                throw new BadHttpRequestException("Tamanho do arquivo maior que 5MB", 413);
            }
            if (file != null && file.Length > 0)
            {
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
                    throw new Exception("Ocorreu algum erro no envio da mensagem");
                }

            }
            return BadRequest("Nenhuma imagem enviada.");
        }

        private static string SetRandomName(string originalName)
        {
            string baseName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            string extension = Path.GetExtension(originalName);
            return $"{baseName}{extension}";
        }
    }
}
