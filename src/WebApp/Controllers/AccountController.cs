namespace WebApp.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Newtonsoft.Json;
	using System.Text;
	using WebApp.Models;

	public class AccountController : Controller
	{
        private readonly string baseUrl;

		public AccountController()
        {
            baseUrl = "http://webapi:80/api/Account/";
		}

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

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
                HttpResponseMessage response = await httpClient.PostAsync(baseUrl + "Register", content);

                if (response.IsSuccessStatusCode)
                {
					return Ok("Account created");
				}
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
			string jsonRegisterViewModel = JsonConvert.SerializeObject(loginViewModel);

			using (var httpClient = new HttpClient())
			{
				var content = new StringContent(jsonRegisterViewModel, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await httpClient.PostAsync(baseUrl + "Login", content);

				if (response.IsSuccessStatusCode)
				{
                    string token = await response.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString("AuthToken", token);
                    if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
				}
			}
			return BadRequest();
		}
    }
}
