namespace WebApi
{
    using Infrastructure.ApplicationContext;
    using Microsoft.EntityFrameworkCore;
    using Producer.Interface;
    using Producer.Service;
    using DotNetEnv;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Env.Load();

            // Add services to the container.
            builder.Services.AddScoped<IKafkaProducerService, KafkaProducerService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            // app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}