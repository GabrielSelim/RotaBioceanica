using Projeto_Gabriel.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Gabriel.Domain.Entity.Financas
{
    [Table("cartoes")]
    public class Cartao : BaseEntity
    {
        [Column("nomeUsuario")]
        [Required]
        public string NomeUsuario { get; set; }

        [Column("nomeBanco")]
        [Required]
        public string NomeBanco { get; set; }

        [Column("usuarioId")]
        [Required]
        public long UsuarioId { get; set; }


        //Entity Relationships
        public Usuario Usuario { get; set; }
        public ICollection<Parcelamento>? Parcelamentos { get; set; }
        public ICollection<ParcelamentoMensal>? ParcelamentosMensais { get; set; }
    }
}