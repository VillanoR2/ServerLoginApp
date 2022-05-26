using Microsoft.EntityFrameworkCore;
using ServerLoginApp.Modelos;

namespace ServerLoginApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
