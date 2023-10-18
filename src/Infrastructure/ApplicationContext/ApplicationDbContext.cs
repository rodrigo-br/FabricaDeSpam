namespace Infrastructure.ApplicationContext
{
    using Domain.Entities;
    using DotNetEnv;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Env.Load();
            base.OnConfiguring(optionsBuilder);

            optionsBuilder
                .UseNpgsql($"Server=postgresdb;" +
                $"Port=5432;" +
                $"Database={Env.GetString("POSTGRES_DB")};" +
                $"User Id={Env.GetString("POSTGRES_USER")};" +
                $"Password={Env.GetString("POSTGRES_PASSWORD")};");
        }
    }
}
