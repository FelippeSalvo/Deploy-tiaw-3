using Npgsql;
using PCraft.Core.Models;
using Microsoft.EntityFrameworkCore;
namespace PCraft.Core.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet <Usuario> Usuarios {get; set;}
    }
}