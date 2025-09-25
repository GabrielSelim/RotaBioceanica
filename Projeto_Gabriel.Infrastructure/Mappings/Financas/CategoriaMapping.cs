using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Infrastructure.Mappings.Financas
{
    public class CategoriaMapping : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            // Configura o nome da tabela
            builder.ToTable("categorias");

            // Configura as propriedades
            builder.Property(e => e.NomeCategoria)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("nomeCategoria");

            builder.Property(e => e.TipoCategoria)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("tipoCategoria");

            // Configura os relacionamentos
            builder.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId);

            builder.HasMany(e => e.Lancamentos)
                .WithOne(e => e.Categoria)
                .HasForeignKey(e => e.CategoriaId);
        }
    }
}