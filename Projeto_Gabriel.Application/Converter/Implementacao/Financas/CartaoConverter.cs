using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.CartaoDbo;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas
{
    public class CartaoConverter : IParser<CartaoDbo, Cartao>, IParser<Cartao, CartaoDbo>
    {
        public Cartao Parse(CartaoDbo origem)
        {
            if (origem == null) return null;
            return new Cartao
            {
                Id = origem.Id,
                NomeUsuario = origem.NomeUsuario,
                NomeBanco = origem.NomeBanco,
                UsuarioId = origem.UsuarioId
            };
        }

        public CartaoDbo Parse(Cartao origem)
        {
            if (origem == null) return null;
            return new CartaoDbo
            {
                Id = origem.Id,
                NomeUsuario = origem.NomeUsuario,
                NomeBanco = origem.NomeBanco,
                UsuarioId = origem.UsuarioId
            };
        }

        public List<Cartao> ParseList(List<CartaoDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }

        public List<CartaoDbo> ParseList(List<Cartao> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}