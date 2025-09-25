using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Infrastructure.Mappings.Financas
{
    public class ParcelamentoMapping : IEntityTypeConfiguration<Parcelamento>
    {
        public void Configure(EntityTypeBuilder<Parcelamento> builder)
        {
            // Configura o nome da tabela
            builder.ToTable("parcelamentos");

            // Configura as propriedades
            builder.Property(e => e.Descricao)
                .HasMaxLength(200)
                .IsRequired()
                .HasColumnName("descricao");

            builder.Property(e => e.ValorTotal)
                .HasColumnType("decimal(10,2)")
                .IsRequired()
                .HasColumnName("valorTotal");

            builder.Property(e => e.NumeroParcelas)
                .IsRequired()
                .HasColumnName("numeroParcelas");

            builder.Property(e => e.DataPrimeiraParcela)
                .IsRequired()
                .HasColumnName("dataPrimeiraParcela");

            builder.Property(e => e.IntervaloParcelas)
                .IsRequired()
                .HasColumnName("intervaloParcelas");

            builder.Property(e => e.Situacao)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnName("situacao");

            // Configura os relacionamentos
            builder.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId);

            builder.HasOne(e => e.Cartao)
                .WithMany(e => e.Parcelamentos)
                .HasForeignKey(e => e.CartaoId);

            builder.HasOne(e => e.PessoaConta)
                .WithMany(e => e.Parcelamentos)
                .HasForeignKey(e => e.PessoaContaId);

            builder.HasMany(e => e.ParcelamentosMensais)
                .WithOne(e => e.Parcelamento)
                .HasForeignKey(e => e.ParcelamentoId);
        }
    }
}