using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Projeto_Gabriel.Infrastructure.Mappings
{
    public class LivrosMapping : IEntityTypeConfiguration<Livros>
    {
        public void Configure(EntityTypeBuilder<Livros> builder)
        {
            // Configura o nome da tabela
            builder.ToTable("books");

            // Configura as propriedades
            builder.Property(e => e.Autor)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("author");

            builder.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("title");

            builder.Property(e => e.DataLancamento)
                .IsRequired()
                .HasColumnName("launch_date");

            builder.Property(e => e.Preco)
                .HasColumnType("decimal(10,2)")
                .IsRequired()
                .HasColumnName("price");

            builder.Property(e => e.Ativo)
                .IsRequired()
                .HasColumnName("enabled");
        }
    }
}