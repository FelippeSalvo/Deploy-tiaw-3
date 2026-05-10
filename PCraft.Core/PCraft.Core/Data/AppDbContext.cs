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
        public DbSet<BuildSalva> BuildsSalvas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuildSalva>(entity =>
            {
                entity.ToTable("BuildsSalvas");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Cpu)
                    .WithMany()
                    .HasForeignKey(e => e.CpuId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Motherboard)
                    .WithMany()
                    .HasForeignKey(e => e.MotherboardId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Ram)
                    .WithMany()
                    .HasForeignKey(e => e.RamId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Gpu)
                    .WithMany()
                    .HasForeignKey(e => e.GpuId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Psu)
                    .WithMany()
                    .HasForeignKey(e => e.PsuId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}