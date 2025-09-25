using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Infrastructure.Mappings.Financas
{
    public class ParcelamentoMensalMapping : IEntityTypeConfiguration<ParcelamentoMensal>
    {
        public void Configure(EntityTypeBuilder<ParcelamentoMensal> builder)
        {
            // Configura o nome da tabela
            builder.ToTable("parcelamentoMensais");

            // Configura as propriedades
            builder.Property(e => e.NumeroParcela)
                .IsRequired()
                .HasColumnName("numeroParcela");

            builder.Property(e => e.DataVencimento)
                .IsRequired()
                .HasColumnName("dataVencimento");

            builder.Property(e => e.DataPagamento)
                .HasColumnName("dataPagamento");

            builder.Property(e => e.ValorParcela)
                .HasColumnType("decimal(10,2)")
                .IsRequired()
                .HasColumnName("valorParcela");

            builder.Property(e => e.ValorPago)
                .HasColumnType("decimal(10,2)")
                .HasColumnName("valorPago");

            builder.Property(e => e.Situacao)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnName("situacao");

            // Configura os relacionamentos
            builder.HasOne(e => e.Parcelamento)
                .WithMany(e => e.ParcelamentosMensais)
                .HasForeignKey(e => e.ParcelamentoId);

            builder.HasOne(e => e.Cartao)
                .WithMany(e => e.ParcelamentosMensais)
                .HasForeignKey(e => e.CartaoId);

            builder.HasOne(e => e.PessoaConta)
                .WithMany(e => e.PagamentosMensais)
                .HasForeignKey(e => e.PessoaContaId);
        }
    }
}