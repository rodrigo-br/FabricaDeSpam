using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ImageController : Controller
    {
        private byte[] image;

        [HttpPost]
        public IActionResult Index([FromBody]byte[] imageData)
        {
            if (imageData != null && imageData.Length > 0)
            {
                image = imageData;
                return Ok("Imagem recebida com sucesso.");
            }
            return NotFound("Dados da imagem ausentes ou inválidos.");
        }

        [HttpGet]
        public IActionResult ExibirImage()
        {
            if (image != null && image.Length > 0)
            {
                return View("ExibirImage", image);
            }
            return NotFound("Nenhuma imagem em memória");
        }
    }
}
