namespace WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
	using WebApp.Models;
    using WebApp.Services.Interfaces;
    using DotNetEnv;
    using Microsoft.AspNetCore.Http;

    public class AccountController : Controller
	{
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
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
            bool registerSucess = await _userService.Register(registerViewModel);
            if (registerSucess)
            {
                return RedirectToAction("Index", "Home");
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
            Env.Load();
            var token = await _userService.Login(loginViewModel);

            if (token == null)
            {
                return BadRequest();
            }

            HttpContext.Session.SetString("AuthToken", token);
            HttpContext.Session.SetString("UserName", loginViewModel.Username);
            HttpContext.Session.SetString("ExpireTime", DateTime.Now.AddMinutes(Env.GetInt("JWT_EXPIRE_TIME")).ToString());
            if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
            {
                return Redirect(loginViewModel.ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
		}
    }
}
