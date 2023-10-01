namespace WebApi
{
    using Infrastructure.ApplicationContext;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using Producer.Interface;
    using Producer.Service;
    using DotNetEnv;
    using Microsoft.AspNetCore.Identity;
    using Domain.Entities;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Scrutor;
    using Infrastructure.Repositories;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>();
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IKafkaProducerService, KafkaProducerService>();
            services.AddScoped<IImageRepository, ImageRepository>();

            services.AddCors(options =>
                options.AddPolicy("NaoSeiOQueColocarAquiAinda", builder =>
                    builder.AllowAnyOrigin().
                    AllowAnyHeader())
            );

            services.Scan(scan =>
                scan.FromApplicationDependencies()
                .AddClasses(x => x.InNamespaces(new string[] { "Infrastructure", "Domain", "Producer" }))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            // app.UseAuthentication();

            app.UseCors("NaoSeiOQueColocarAquiAinda");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }        
    }
}
