using Microsoft.EntityFrameworkCore;
using APIService.Models;

namespace APIService.Data 
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Prestacao> Prestacoes { get; set; }
    }
}