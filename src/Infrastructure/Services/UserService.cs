namespace WebApi.Services
{
	using Domain.DTO;
	using Domain.Entities;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;

	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _tokenService = tokenService;
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

		public async Task<string?> LoginUser(LoginDTO loginDTO)
		{
			var signInResult = await _signInManager.PasswordSignInAsync(loginDTO.Username, loginDTO.Password, false, false);

			if (signInResult != null && signInResult.Succeeded)
			{
				var user = _signInManager
					.UserManager
					.Users
					.FirstOrDefault(u => u.NormalizedUserName == loginDTO.Username.ToUpper());

				if (user != null)
				{
					return _tokenService.GenerateToken(user);
				}
			}

			return null;
		}
	}
}
