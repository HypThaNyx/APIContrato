using Microsoft.EntityFrameworkCore;
using APIService.Models;

namespace APIService.Data 
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contrato>()
                .HasMany(c => c.Prestacoes)
                .WithOne(p => p.Contrato)
                .HasForeignKey(p => p.IdContrato)
                .IsRequired();
        }

        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Prestacao> Prestacoes { get; set; }
    }
}