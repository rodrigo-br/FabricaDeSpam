using WebApp.Models;

namespace WebApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<HttpResponseMessage?> GetUserId(string? token);
        Task<string?> Login(LoginViewModel loginViewModel);
        Task<bool> Register(RegisterViewModel registerViewModel);
    }
}
