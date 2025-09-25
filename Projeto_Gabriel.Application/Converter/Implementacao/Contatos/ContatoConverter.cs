using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Contatos;
using Projeto_Gabriel.Domain.Entity.Contatos;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Contatos
{
    public class ContatoConverter : IParser<Contato, ContatoDbo>, IParser<ContatoDbo, Contato>
    {
        public Contato Parse(ContatoDbo origem)
        {
            if (origem == null) return null;
            return new Contato
            {
                Id = origem.Id,
                Nome = origem.Nome,
                Email = origem.Email,
                Mensagem = origem.Mensagem,
                DataContato = origem.DataContato,
                UsuarioId = origem.UsuarioId
            };
        }

        public ContatoDbo Parse(Contato origem)
        {
            if (origem == null) return null;
            return new ContatoDbo
            {
                Id = origem.Id,
                Nome = origem.Nome,
                Email = origem.Email,
                Mensagem = origem.Mensagem,
                DataContato = origem.DataContato,
                UsuarioId = origem.UsuarioId
            };
        }

        public List<Contato> ParseList(List<ContatoDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }

        public List<ContatoDbo> ParseList(List<Contato> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}