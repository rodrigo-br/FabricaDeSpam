namespace Infrastructure.Repositories
{
	using Domain.DTO;

	public interface IUserRepository
	{
		Task<bool> Register(RegisterDTO registerDTO);
	}
}
