namespace WebApi.Controllers
{
    using Domain.DTO;
    using Infrastructure.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Producer.Interface;

    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly IImageRepository _imageRepository;

        public ProducerController(IKafkaProducerService kafkaProducer, IImageRepository imageRepository)
        {
            _kafkaProducer = kafkaProducer;
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<ImageDTO?> GetById(Guid id)
        {
            return await _imageRepository.GetById(id);
        }

        [HttpPost]
        [Route("cat")]
        public async Task<IActionResult> UploadFile([FromBody]ImageDTO imageDTO)
        {
            foreach (var topic in imageDTO.Topics)
            {
                bool success = await _kafkaProducer.ProduceMessageAsync("cat", imageDTO.FileName, imageDTO.ImageData);
                Console.WriteLine(imageDTO.FileName);
                if (!success)
                {
                    return StatusCode(500, "Ocorreu algum erro no envio da mensagem");
                }
            }
            return Ok("Tudo certo por aqui ;D");
        }
    }
}
