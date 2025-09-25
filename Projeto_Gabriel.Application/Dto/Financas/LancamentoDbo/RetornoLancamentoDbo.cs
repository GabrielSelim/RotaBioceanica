using Projeto_Gabriel.Application.Hypermedia;
using Projeto_Gabriel.Application.Hypermedia.Abstract;

namespace Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo
{
    public class RetornoLancamentoDbo : ISupportHyperMedia
    {
        public long Id { get; set; }

        public DateTime DataLancamento { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public string TipoLancamento { get; set; }

        public long CategoriaId { get; set; }

        public long? ParcelamentoMensalId { get; set; }

        public string Situacao { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}