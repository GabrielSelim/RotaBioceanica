using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto_Gabriel.Domain.Entity.Contatos;

namespace Projeto_Gabriel.Infrastructure.Mappings
{
    public class ContatoMapping : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            // Configura o nome da tabela
            builder.ToTable("contatos");

            // Configura as propriedades
            builder.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("nome");

            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("email");

            builder.Property(e => e.Mensagem)
                .HasMaxLength(1000)
                .IsRequired()
                .HasColumnName("mensagem");

            builder.Property(e => e.DataContato)
                .IsRequired()
                .HasColumnName("dataContato");

            // Configura o relacionamento com Usuario
            builder.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}