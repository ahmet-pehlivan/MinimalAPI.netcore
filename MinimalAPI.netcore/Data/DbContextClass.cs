using Microsoft.EntityFrameworkCore;
using MinimalAPI.netcore.Entities;

namespace MinimalAPI.netcore.Data
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Product> Product { get; set; }
    }
}