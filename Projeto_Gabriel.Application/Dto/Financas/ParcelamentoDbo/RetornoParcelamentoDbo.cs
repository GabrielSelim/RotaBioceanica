using Projeto_Gabriel.Application.Hypermedia;
using Projeto_Gabriel.Application.Hypermedia.Abstract;

namespace Projeto_Gabriel.Application.Dto.Financas.ParcelamentoDbo
{
    public class RetornoParcelamentoDbo : ISupportHyperMedia
    {
        public long Id { get; set; }

        public string Descricao { get; set; }

        public decimal ValorTotal { get; set; }

        public int NumeroParcelas { get; set; }

        public DateTime DataPrimeiraParcela { get; set; }

        public int IntervaloParcelas { get; set; }

        public long? CartaoId { get; set; }

        public long? PessoaContaId { get; set; }

        public string Situacao { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}