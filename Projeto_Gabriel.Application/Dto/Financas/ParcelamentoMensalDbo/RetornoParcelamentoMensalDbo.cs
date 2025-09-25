using Projeto_Gabriel.Application.Hypermedia;
using Projeto_Gabriel.Application.Hypermedia.Abstract;

namespace Projeto_Gabriel.Application.Dto.Financas.ParcelamentoMensalDbo
{
    public class RetornoParcelamentoMensalDbo : ISupportHyperMedia
    {
        public long Id { get; set; }
        public long ParcelamentoId { get; set; }
        public int NumeroParcela { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public decimal ValorParcela { get; set; }
        public decimal? ValorPago { get; set; }
        public long? CartaoId { get; set; }
        public long? PessoaContaId { get; set; }
        public string Situacao { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}