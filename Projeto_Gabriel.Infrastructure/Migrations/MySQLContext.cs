using Microsoft.EntityFrameworkCore;
using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.Entity.Contatos;
using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.Entity.Logas;

namespace Projeto_Gabriel.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext()
        {

        }

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options){}

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Livros> Livros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CartaPokemon> CartasPokemon { get; set; }

        //Finanças
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cartao> Cartoes { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<Parcelamento> Parcelamentos { get; set; }
        public DbSet<ParcelamentoMensal> ParcelamentoMensais { get; set; }
        public DbSet<PessoaConta> PessoaContas { get; set; }

        //Logs
        public DbSet<LogEntry> Logs { get; set; }

        //Contato 
        public DbSet<Contato> Contatos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica todas as configurações de mapeamento encontradas no assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MySQLContext).Assembly);
        }
    }
}