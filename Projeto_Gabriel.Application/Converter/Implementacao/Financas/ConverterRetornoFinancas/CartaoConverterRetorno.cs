using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.CartaoDbo;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas
{
    public class CartaoConverterRetorno : IParser<CartaoDbo, RetornoCartaoDbo>, IParser<RetornoCartaoDbo, CartaoDbo>
    {
        public RetornoCartaoDbo Parse(CartaoDbo origem)
        {
            if (origem == null) return null;
            return new RetornoCartaoDbo
            {
                Id = origem.Id,
                NomeUsuario = origem.NomeUsuario,
                NomeBanco = origem.NomeBanco
            };
        }
        public CartaoDbo Parse(RetornoCartaoDbo origem)
        {
            if (origem == null) return null;
            return new CartaoDbo
            {
                Id = origem.Id,
                NomeUsuario = origem.NomeUsuario,
                NomeBanco = origem.NomeBanco
            };
        }
        public List<RetornoCartaoDbo> ParseList(List<CartaoDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
        public List<CartaoDbo> ParseList(List<RetornoCartaoDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}