//namespace Infrastructure.Repositories
//{
//	using Domain.DTO;
//	using Microsoft.AspNetCore.Identity;

//	public class UserRepository : IUserRepository
//	{
//		public async Task<bool> Register(RegisterDTO registerDTO)
//		{
//			Console.WriteLine($"{registerDTO.Username}, {registerDTO.Email}, {registerDTO.Password}");
//			var identityUser = new IdentityUser
//			{
//				UserName = registerDTO.Username,
//				Email = registerDTO.Email,
//			};

//			var identityResult = await _userManager.CreateAsync(identityUser, registerDTO.Password);

//			if (identityResult.Succeeded)
//			{
//				return true;
//			}
//			return false;
//		}
//	}
//}
