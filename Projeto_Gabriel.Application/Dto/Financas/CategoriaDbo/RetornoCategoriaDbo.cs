using Projeto_Gabriel.Application.Hypermedia;
using Projeto_Gabriel.Application.Hypermedia.Abstract;

namespace Projeto_Gabriel.Application.Dto.Financas.CategoriaDbo
{
    public class RetornoCategoriaDbo : ISupportHyperMedia
    {
        public long Id { get; set; }

        public string NomeCategoria { get; set; }

        public string TipoCategoria { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}