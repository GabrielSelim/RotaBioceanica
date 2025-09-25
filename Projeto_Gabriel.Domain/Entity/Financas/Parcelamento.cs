using Projeto_Gabriel.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Gabriel.Domain.Entity.Financas
{
    [Table("parcelamentos")]
    public class Parcelamento : BaseEntity
    {
        [Column("usuarioId")]
        [Required]
        public long UsuarioId { get; set; }

        [Column("descricao")]
        [Required]
        public string Descricao { get; set; }

        [Column("valorTotal")]
        [Required]
        public decimal ValorTotal { get; set; }

        [Column("numeroParcelas")]
        [Required]
        public int NumeroParcelas { get; set; }

        [Column("dataPrimeiraParcela")]
        [Required]
        public DateTime DataPrimeiraParcela { get; set; }

        [Column("intervaloParcelas")]
        [Required]
        public int IntervaloParcelas { get; set; }

        [Column("cartaoId")]
        public long? CartaoId { get; set; }

        [Column("pessoaContaId")]
        public long? PessoaContaId { get; set; }

        [Column("situacao")]
        [Required]
        public string Situacao { get; set; }


        //Entity Relationships
        public ICollection<ParcelamentoMensal>? ParcelamentosMensais { get; set; }
        public Usuario Usuario { get; set; }
        public Cartao? Cartao { get; set; }
        public PessoaConta? PessoaConta { get; set; }
    }
}