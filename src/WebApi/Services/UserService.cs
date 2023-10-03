namespace WebApi.Services
{
	using Domain.DTO;
	using Domain.Entities;
	using Microsoft.AspNetCore.Identity;

	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
			_userManager = userManager;
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
	}
}
