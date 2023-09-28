using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Polly;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly string directory;
        private IProducer<string, byte[]>? kafkaProducer;

        public ProducerController()
        {
            directory = Path.Combine(Directory.GetCurrentDirectory(), "files");
            double timeSpan = 5;

            var retryPolicy = Policy
            .Handle<Exception>() // Ajuste isso para o tipo de exceção que você deseja tratar.
            .WaitAndRetryForever(
                retryAttempt => TimeSpan.FromSeconds(timeSpan), // Intervalo entre as tentativas.
                (exception, timeSpan, context) =>
                {
                    Console.WriteLine($"Erro ao se conectar ao Kafka. Tentando novamente em {timeSpan} segundos.");
                });

            retryPolicy.Execute(() =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "kafka:9092",
                    MessageMaxBytes = 5000000
                };
                kafkaProducer = new ProducerBuilder<string, byte[]>(config).Build();
            });
        }

        [HttpPost]
        [Route("cat")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (kafkaProducer == null)
            {
                throw new Exception("Conexão com o kafka não estabelecida, contacte o administrador para mais informações");
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
