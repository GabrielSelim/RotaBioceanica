using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Infrastructure.Mappings.Financas
{
    public class LancamentoMapping : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            // Configura o nome da tabela
            builder.ToTable("lancamentos");

            // Configura as propriedades
            builder.Property(e => e.DataLancamento)
                .IsRequired()
                .HasColumnName("dataLancamento");

            builder.Property(e => e.Descricao)
                .HasMaxLength(200)
                .IsRequired()
                .HasColumnName("descricao");

            builder.Property(e => e.Valor)
                .HasColumnType("decimal(10,2)")
                .IsRequired()
                .HasColumnName("valor");

            builder.Property(e => e.TipoLancamento)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("tipoLancamento");

            builder.Property(e => e.Situacao)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnName("situacao");

            // Configura os relacionamentos
            builder.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId);

            builder.HasOne(e => e.Categoria)
                .WithMany(e => e.Lancamentos)
                .HasForeignKey(e => e.CategoriaId);

            builder.HasOne(e => e.ParcelamentoMensal)
                .WithMany()
                .HasForeignKey(e => e.ParcelamentoMensalId);
        }
    }
}