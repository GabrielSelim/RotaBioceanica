using Projeto_Gabriel.Application.Hypermedia;
using Projeto_Gabriel.Application.Hypermedia.Abstract;

namespace Projeto_Gabriel.Application.Dto.Contatos
{
    public class RetornoContatoDbo : ISupportHyperMedia
    {
        public long Id { get; set; }
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Mensagem { get; set; }

        public DateTime DataContato { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
