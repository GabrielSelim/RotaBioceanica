using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Infrastructure.Mappings.Financas
{
    public class CartaoMapping : IEntityTypeConfiguration<Cartao>
    {
        public void Configure(EntityTypeBuilder<Cartao> builder)
        {
            // Configura o nome da tabela
            builder.ToTable("cartoes");

            // Configura as propriedades
            builder.Property(e => e.NomeUsuario)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("nomeUsuario");

            builder.Property(e => e.NomeBanco)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("nomeBanco");

            // Configura os relacionamentos
            builder.HasMany(e => e.Parcelamentos)
                .WithOne(e => e.Cartao)
                .HasForeignKey(e => e.CartaoId);

            builder.HasMany(e => e.ParcelamentosMensais)
                .WithOne(e => e.Cartao)
                .HasForeignKey(e => e.CartaoId);
        }
    }
}