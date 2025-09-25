using Projeto_Gabriel.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Gabriel.Domain.Entity.Financas
{
    [Table("categorias")]
    public class Categoria : BaseEntity
    {
        [Column("usuarioId")]
        [Required]
        public long UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        [Column("nomeCategoria")]
        [Required]
        public string NomeCategoria { get; set; }

        [Column("tipoCategoria")]
        [Required]
        public string TipoCategoria { get; set; }


        //Entity Relationships
        public ICollection<Lancamento>? Lancamentos { get; set; }
    }
}