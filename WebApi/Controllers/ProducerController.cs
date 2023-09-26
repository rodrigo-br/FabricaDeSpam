using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly string directory;
        private readonly IProducer<string, byte[]> kafkaProducer;

        public ProducerController()
        {
            // Configurar a pasta de destino
            directory = Path.Combine(Directory.GetCurrentDirectory(), "files");

            // Configurar o produtor Kafka (substitua pelos detalhes do seu ambiente)
            var config = new ProducerConfig
            {
                BootstrapServers = "kafka:29092",
                MessageMaxBytes = 5000000
            };
            kafkaProducer = new ProducerBuilder<string, byte[]>(config).Build();
        }

        [HttpPost]
        [Route("cat")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
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
                kafkaProducer.Produce("cat", new Message<string, byte[]>
                {
                    Key = randomName, 
                    Value = fileData
                });

                return Ok(new { mensagem = "Imagem salva com sucesso!" });
            }
            return BadRequest("Nenhuma imagem enviada.");
        }

        private string SetRandomName(string originalName)
        {
            string baseName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            string extension = Path.GetExtension(originalName);
            return $"{baseName}{extension}";
        }
    }
}
