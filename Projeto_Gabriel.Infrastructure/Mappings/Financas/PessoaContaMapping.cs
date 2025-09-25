using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Infrastructure.Mappings.Financas
{
    public class PessoaContaMapping : IEntityTypeConfiguration<PessoaConta>
    {
        public void Configure(EntityTypeBuilder<PessoaConta> builder)
        {
            // Configura o nome da tabela
            builder.ToTable("pessoaContas");

            // Configura as propriedades
            builder.Property(e => e.NomePessoa)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("nomePessoa");

            // Configura os relacionamentos
            builder.HasMany(e => e.Parcelamentos)
                .WithOne(e => e.PessoaConta)
                .HasForeignKey(e => e.PessoaContaId);

            builder.HasMany(e => e.PagamentosMensais)
                .WithOne(e => e.PessoaConta)
                .HasForeignKey(e => e.PessoaContaId);
        }
    }
}