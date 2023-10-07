using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ImageController : Controller
    {
        private byte[] image;

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

		[HttpGet]
		public IActionResult Receiver()
		{
            return View();
		}
	}
}
