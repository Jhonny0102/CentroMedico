using CentroMedico.Models;
using Microsoft.EntityFrameworkCore;

namespace CentroMedico.Data
{
    public class CentroMedicoContext : DbContext
    {
        public CentroMedicoContext(DbContextOptions<CentroMedicoContext> options)
            :base(options)
        {}

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
