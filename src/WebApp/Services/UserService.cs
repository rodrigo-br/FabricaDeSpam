using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly string userBaseUrl = "http://webapi:80/api/User/";
        private readonly string baseUrl = "http://webapi:80/api/Account/";

        public async Task<HttpResponseMessage?> GetUserId(string? token)
        {
            if (token == null)
            {
                return null;
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage idResponse = await httpClient.GetAsync(userBaseUrl + "IdClaimer");
                return idResponse.IsSuccessStatusCode ? idResponse : null;
            }
        }

        public async Task<string?> Login(LoginViewModel loginViewModel)
        {
            string jsonRegisterViewModel = JsonConvert.SerializeObject(loginViewModel);

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(jsonRegisterViewModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(baseUrl + "Login", content);

                return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
            }
        }

        public async Task<bool> Register(RegisterViewModel registerViewModel)
        {
            string jsonRegisterViewModel = JsonConvert.SerializeObject(registerViewModel);

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(jsonRegisterViewModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(baseUrl + "Register", content);

                return response.IsSuccessStatusCode;
            }
        }
    }
}
