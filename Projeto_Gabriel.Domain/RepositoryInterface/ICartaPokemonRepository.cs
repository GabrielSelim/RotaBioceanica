using Microsoft.AspNetCore.Http;
using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface
{
    public interface ICartaPokemonRepository : IRepository<CartaPokemon>
    {
        Task<List<CartaPokemon>> SaveImagesToDatabase(List<IFormFile> files);

        List<CartaPokemon> FiltrarPorCriterios(
            string? nome = null,
            string? tipo = null,
            string? raridade = null,
            string? estagio = null,
            string? versao = null,
            string? booster = null,
            string? sortField = null,
            string? sortDirection = "asc"
        );

        void AtivarCarta(long id);

        void InativarCarta(long id);
    }
}