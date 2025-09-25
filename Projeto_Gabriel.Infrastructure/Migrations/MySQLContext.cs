using Microsoft.EntityFrameworkCore;
using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.Entity.Logas;

namespace Projeto_Gabriel.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext()
        {

        }

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options){}

        public DbSet<Livros> Livros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }


        //Logs
        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica todas as configurações de mapeamento encontradas no assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MySQLContext).Assembly);
        }
    }
}