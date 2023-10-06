using Domain.Entities;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            Env.Load();

            Claim[] claims = new Claim[]
            {
                new Claim("username", user.UserName),
                new Claim("id", user.Id)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    Env.GetString("JWT_SECRET")));

            var signingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddMinutes(30),
                    claims: claims,
                    signingCredentials: signingCredentials
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
