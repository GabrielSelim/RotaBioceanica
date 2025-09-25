using Projeto_Gabriel.Application.Hypermedia;
using Projeto_Gabriel.Application.Hypermedia.Abstract;

namespace Projeto_Gabriel.Application.Dto.Financas.PessoaContaDbo
{
    public class RetornoPessoaContaDbo : ISupportHyperMedia
    {
        public long Id { get; set; }

        public string NomePessoa { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}