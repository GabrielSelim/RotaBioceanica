using Microsoft.AspNetCore.Http;
using Projeto_Gabriel.Application.Dto;

namespace Projeto_Gabriel.Bussines
{
    public interface IPokemonFileBusiness
    {
        Task<List<CartaPokemonDbo>> SavePokemonImagesToDatabase(List<IFormFile> files);
        List<CartaPokemonDbo> FiltrarPorCriterios( string? nome = null, string? tipo = null, string? raridade = null, string? estagio = null, string? versao = null, string? booster = null, string? sortField = null, string? sortDirection = "asc");

        void Deletar(long id);

        List<CartaPokemonDbo> ObterTodosSemImagem();

        void AtivarCarta(long id);

        void InativarCarta(long id);

        CartaPokemonDbo ObterPorId(long id);

        CartaPokemonDbo Atualizar(CartaPokemonDbo pessoa);
    }
}