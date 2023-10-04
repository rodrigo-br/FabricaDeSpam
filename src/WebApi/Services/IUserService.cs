namespace WebApi.Services
{
	using Domain.DTO;
	using Domain.Entities;

	public interface IUserService
	{
		Task<bool> RegisterUser(RegisterDTO registerDTO);
		Task<bool> LoginUser(LoginDTO loginDTO);
	}
}
