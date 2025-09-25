using Projeto_Gabriel.Application.Hypermedia;
using Projeto_Gabriel.Application.Hypermedia.Abstract;

namespace Projeto_Gabriel.Application.Dto.Financas.CartaoDbo
{
    public class RetornoCartaoDbo : ISupportHyperMedia
    {
        public long Id { get; set; }

        public string NomeUsuario { get; set; }

        public string NomeBanco { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}