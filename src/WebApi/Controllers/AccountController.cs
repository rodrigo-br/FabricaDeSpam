namespace WebApi.Controllers
{
	using Domain.DTO;
	using Domain.Entities;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<User> _userManager;

		public AccountController(UserManager<User> userManager)
        {
			_userManager = userManager;
		}

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
		{
			Console.WriteLine($"{registerDTO.Username}, {registerDTO.Email}, {registerDTO.Password}");
			var identityUser = new User
			{
				UserName = registerDTO.Username,
				Email = registerDTO.Email,
			};

			var identityResult = await _userManager.CreateAsync(identityUser, registerDTO.Password);

			if (identityResult.Succeeded)
			{
				return Ok(identityResult);
			}
			return BadRequest();
		}
    }
}
