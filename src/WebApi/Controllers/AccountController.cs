namespace WebApi.Controllers
{
	using Domain.DTO;
	using Microsoft.AspNetCore.Mvc;
	using WebApi.Services;

	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IUserService _userService;

		public AccountController(IUserService userService)
        {
			_userService = userService;
		}

        [HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
		{
			bool succeed = await _userService.RegisterUser(registerDTO);

			if (succeed)
			{
				return Ok();
			}
			return BadRequest();
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
		{
			if (ModelState.IsValid)
			{
				bool succeed = await _userService.LoginUser(loginDTO);

				if (succeed)
				{
					return Ok();
				}

			}
			return BadRequest();
		}
	}
}
