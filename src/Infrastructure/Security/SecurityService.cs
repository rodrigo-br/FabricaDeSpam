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
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;

    public static class SecurityService
	{
		public static void AddAppIdentity(this IServiceCollection services)
		{
			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
		}

		public static IHost MigrateDatabase<T>(this IHost webHost) where T : DbContext
		{
			using (var scope = webHost.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var db = services.GetRequiredService<T>();
					db.Database.Migrate();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred while migrating the database: {ex.ToString()}");
				}
			}
			return webHost;
		}

		public static void AddAppAuthentication(this IServiceCollection services)
		{
			Env.Load();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.SaveToken = true;
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						RequireExpirationTime = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET"))),
						ValidateIssuerSigningKey = true,
						ValidateAudience = false,
						ValidateIssuer = false,
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero
					};
				});
		}
	}
}
