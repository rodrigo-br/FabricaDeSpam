namespace WebApi
{
    using Infrastructure.ApplicationContext;
    using Microsoft.Extensions.DependencyInjection;
    using Producer.Interface;
    using Producer.Service;
    using Domain.Entities;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Scrutor;
    using Infrastructure.Repositories;
    using System.Text.Json.Serialization;
    using Infrastructure.Profiles;
	using WebApi.Services;
	using Infrastructure.Security;

	public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
            services.AddDbContext<ApplicationDbContext>();
            services.AddAppIdentity();
            services.AddAppAuthentication();

            services.Scan(scan =>
                scan.FromApplicationDependencies()
                .AddClasses(x => x.InNamespaces(new string[] { "Infrastructure", "Producer" }))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
            services.AddAutoMapper(typeof(Startup), typeof(ImageProfile));

            services.AddScoped<IKafkaProducerService, KafkaProducerService>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddCors(options =>
                options.AddPolicy("NaoSeiOQueColocarAquiAinda", builder =>
                    builder.AllowAnyOrigin().
                    AllowAnyHeader())
            );


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
