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

        public DbSet<Contrato> Contrato { get; set; }
        public DbSet<Prestacao> Prestacao { get; set; }
    }
}