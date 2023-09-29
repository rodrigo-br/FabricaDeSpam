using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Polly;
using Producer.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly string directory;
        private readonly IKafkaProducerService _kafkaProducer;

        public ProducerController(IKafkaProducerService kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
            directory = Path.Combine(Directory.GetCurrentDirectory(), "files");
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
