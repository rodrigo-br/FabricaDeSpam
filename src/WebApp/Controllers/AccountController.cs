namespace WebApp.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Newtonsoft.Json;
	using System.Text;
	using WebApp.Models;

	public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            string jsonRegisterViewModel = JsonConvert.SerializeObject(registerViewModel);

            using (var httpClient =  new HttpClient())
            {
                var content = new StringContent(jsonRegisterViewModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("http://webapi:80/api/Account/Register", content);

                if (response.IsSuccessStatusCode)
                {
					return Ok("cavalinho");
				}
            }
            return BadRequest();
        }
    }
}
