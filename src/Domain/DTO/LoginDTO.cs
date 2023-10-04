using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
	public class LoginDTO
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
