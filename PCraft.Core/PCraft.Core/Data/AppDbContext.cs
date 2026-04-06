using Microsoft.EntityFrameworkCore;
using PCraft.Core.Models;

namespace PCraft.Core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CPU> CPUs { get; set; }
        public DbSet<Motherboard> Motherboards { get; set; }
        public DbSet<RAM> RAMs { get; set; }
        public DbSet<GPU> GPUs { get; set; }
        public DbSet<PSU> PSUs { get; set; }
    }
}