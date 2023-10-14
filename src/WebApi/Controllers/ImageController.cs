using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
        [Route("AllImages")]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAll();

			if (images == null || !images.Any())
			{
				return NotFound();
			}

			return Ok(images);
		}
    }
}
