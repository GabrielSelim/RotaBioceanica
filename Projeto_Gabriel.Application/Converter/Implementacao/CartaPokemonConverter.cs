using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Domain.Entity;

namespace Projeto_Gabriel.Application.Converter.Implementacao
{
    public class CartaPokemonConverter : IParser<CartaPokemonDbo, CartaPokemon>, IParser<CartaPokemon, CartaPokemonDbo>
    {
        public CartaPokemon Parse(CartaPokemonDbo origem)
        {
            if (origem == null) return null;

            return new CartaPokemon
            {
                Id = origem.Id,
                NomeVersao = origem.NomeVersao,
                Versao = origem.Versao,
                NumeroCarta = origem.NumeroCarta,
                NomePokemon = origem.NomePokemon,
                Raridade = origem.Raridade,
                Tipo = origem.Tipo,
                HP = origem.HP,
                Estagio = origem.Estagio,
                Booster = origem.Booster,
                Imagem = origem.Imagem,
                Situacao = origem.Situacao
            };
        }

        public CartaPokemonDbo Parse(CartaPokemon origem)
        {
            if (origem == null) return null;

            return new CartaPokemonDbo
            {
                Id = origem.Id,
                NomeVersao = origem.NomeVersao,
                Versao = origem.Versao,
                NumeroCarta = origem.NumeroCarta,
                NomePokemon = origem.NomePokemon,
                Raridade = origem.Raridade,
                Tipo = origem.Tipo,
                HP = origem.HP,
                Estagio = origem.Estagio,
                Booster = origem.Booster,
                Imagem = origem.Imagem,
                Situacao = origem.Situacao
            };
        }

        public List<CartaPokemon> ParseList(List<CartaPokemonDbo> origem)
        {
            if (origem == null) return null;

            return origem.Select(item => Parse(item)).ToList();
        }

        public List<CartaPokemonDbo> ParseList(List<CartaPokemon> origem)
        {
            if (origem == null) return null;

            return origem.Select(item => Parse(item)).ToList();
        }
    }
}