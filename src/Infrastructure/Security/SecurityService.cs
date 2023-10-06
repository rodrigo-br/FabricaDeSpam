namespace Infrastructure.Security
{
	using Microsoft.Extensions.DependencyInjection;
	using Domain.Entities;
	using Infrastructure.ApplicationContext;
	using Microsoft.AspNetCore.Identity;
	using DotNetEnv;
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.IdentityModel.Tokens;
	using System.Text;

	public static class SecurityService
	{
		public static void AddAppIdentity(this IServiceCollection services)
		{
			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
		}

		public static void AddAppAuthentication(this IServiceCollection services)
		{
			Env.Load();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.SaveToken = true;
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidAudience = Env.GetString("JWT_VALID_AUDIENCE"),
						ValidIssuer = Env.GetString("JWT_VALID_ISSUER"),
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET")))
					};
				});
		}
	}
}
