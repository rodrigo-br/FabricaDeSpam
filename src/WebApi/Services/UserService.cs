namespace WebApi.Services
{
	using Domain.DTO;
	using Domain.Entities;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;

	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}

        public async Task<bool> RegisterUser(RegisterDTO registerDTO)
		{
			var identityUser = new User
			{
				UserName = registerDTO.Username,
				Email = registerDTO.Email,
			};

			var identityResult = await _userManager.CreateAsync(identityUser, registerDTO.Password);

			return identityResult.Succeeded;
		}

		public async Task<bool> LoginUser(LoginDTO loginDTO)
		{
			var signInResult = await _signInManager.PasswordSignInAsync(loginDTO.Username, loginDTO.Password, false, false);

			return signInResult != null && signInResult.Succeeded;
		}
	}
}
