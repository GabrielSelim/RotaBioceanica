using Projeto_Gabriel.Application.Hypermedia;
using Projeto_Gabriel.Application.Hypermedia.Abstract;

namespace Projeto_Gabriel.Application.Dto
{
    public class PessoaDbo : ISupportHyperMedia
    {
        public long Id { get; set; }

        public string PrimeiroNome { get; set; }

        public string Sobrenome { get; set; }

        public string Endereco { get; set; }
        
        public string Sexo { get; set; }

        public bool Ativo { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}