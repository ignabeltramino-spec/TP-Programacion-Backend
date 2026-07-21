using Microsoft.EntityFrameworkCore;

namespace Tp_ProgramacionIII.Models
{
    public class AppDbContext : DbContext
    {
       public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transacciones {  get; set; }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
