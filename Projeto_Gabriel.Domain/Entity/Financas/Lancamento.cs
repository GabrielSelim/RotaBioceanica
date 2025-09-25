using Projeto_Gabriel.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Gabriel.Domain.Entity.Financas
{
    [Table("lancamentos")]
    public class Lancamento : BaseEntity
    {
        [Column("usuarioId")]
        [Required]
        public long UsuarioId { get; set; }

        [Column("dataLancamento")]
        [Required]
        public DateTime DataLancamento { get; set; }

        [Column("descricao")]
        [Required]
        public string Descricao { get; set; }

        [Column("valor")]
        [Required]
        public decimal Valor { get; set; }

        [Column("tipoLancamento")]
        [Required]
        public string TipoLancamento { get; set; }

        [Column("categoriaId")]
        [Required]
        public long CategoriaId { get; set; }

        [Column("ParcelamentoMensalId")]
        public long? ParcelamentoMensalId { get; set; }

        [Column("situacao")]
        [Required]
        public string Situacao { get; set; }


        //Entity Relationships
        public Usuario Usuario { get; set; }
        public Categoria Categoria { get; set; }
        public ParcelamentoMensal? ParcelamentoMensal { get; set; }
    }
}