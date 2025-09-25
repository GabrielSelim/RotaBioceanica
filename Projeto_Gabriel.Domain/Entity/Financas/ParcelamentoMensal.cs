using Projeto_Gabriel.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Gabriel.Domain.Entity.Financas
{
    [Table("parcelamentoMensais")]
    public class ParcelamentoMensal : BaseEntity
    {
        [Column("parcelamentoId")]
        [Required]
        public long ParcelamentoId { get; set; }

        [Column("numeroParcela")]
        [Required]
        public int NumeroParcela { get; set; }

        [Column("dataVencimento")]
        [Required]
        public DateTime DataVencimento { get; set; }

        [Column("dataPagamento")]
        public DateTime? DataPagamento { get; set; }

        [Column("valorParcela")]
        [Required]
        public decimal ValorParcela { get; set; }

        [Column("valorPago")]
        public decimal? ValorPago { get; set; }

        [Column("cartaoId")]
        public long? CartaoId { get; set; }

        [Column("pessoaContaId")]
        public long? PessoaContaId { get; set; }

        [Column("situacao")]
        [Required]
        public string Situacao { get; set; }


        //Entity Relationships
        public Cartao? Cartao { get; set; }
        public Parcelamento Parcelamento { get; set; }
        public PessoaConta? PessoaConta { get; set; }
    }
}