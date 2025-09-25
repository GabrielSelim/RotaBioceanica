using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Contatos;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Contatos.ConverterRetorno
{
    public class RetornoConveterContato : IParser<ContatoDbo, RetornoContatoDbo>, IParser<RetornoContatoDbo, ContatoDbo>
    {
        public RetornoContatoDbo Parse(ContatoDbo origem)
        {
            if (origem == null) return null;
            return new RetornoContatoDbo
            {
                Id = origem.Id,
                Nome = origem.Nome,
                Email = origem.Email,
                Mensagem = origem.Mensagem,
                DataContato = origem.DataContato
            };
        }

        public ContatoDbo Parse(RetornoContatoDbo origem)
        {
            if (origem == null) return null;
            return new ContatoDbo
            {
                Id = origem.Id,
                Nome = origem.Nome,
                Email = origem.Email,
                Mensagem = origem.Mensagem,
                DataContato = origem.DataContato
            };
        }

        public List<RetornoContatoDbo> ParseList(List<ContatoDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }

        public List<ContatoDbo> ParseList(List<RetornoContatoDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}