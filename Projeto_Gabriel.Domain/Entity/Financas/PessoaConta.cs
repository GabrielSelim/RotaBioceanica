using Projeto_Gabriel.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Gabriel.Domain.Entity.Financas
{
    [Table("pessoaContas")]
    public class PessoaConta : BaseEntity
    {
        [Column("nomePessoa")]
        [Required]
        public string NomePessoa { get; set; }

        [Column("usuarioId")]
        [Required]
        public long UsuarioId { get; set; }


        //Entity Relationships
        public Usuario Usuario { get; set; }
        public ICollection<Parcelamento>? Parcelamentos { get; set; }
        public ICollection<ParcelamentoMensal>? PagamentosMensais { get; set; }
    }
}