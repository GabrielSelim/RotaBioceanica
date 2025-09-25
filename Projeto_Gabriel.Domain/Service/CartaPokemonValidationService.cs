using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Infrastructure.Services.Validation
{
    public class CartaPokemonValidationService : IEntityValidationService<CartaPokemon>
    {
        public void Validate(CartaPokemon cartaPokemon)
        {
            if (string.IsNullOrWhiteSpace(cartaPokemon.NomeVersao))
                throw new ValidationException("O campo 'Nome da Versão' é obrigatório.");

            if (cartaPokemon.NomeVersao?.Length > 100)
                throw new ValidationException("O campo 'Nome da Versão' deve ter no máximo 50 caracteres.");

            if (string.IsNullOrWhiteSpace(cartaPokemon.Versao))
                throw new ValidationException("O campo 'Versão' é obrigatório.");

            if (cartaPokemon.Versao?.Length > 100)
                throw new ValidationException("O campo 'Versão' deve ter no máximo 50 caracteres.");

            if (string.IsNullOrWhiteSpace(cartaPokemon.NomePokemon))
                throw new ValidationException("O campo 'Nome do Pokémon' é obrigatório.");

            if (cartaPokemon.NomePokemon?.Length > 100)
                throw new ValidationException("O campo 'Nome do Pokémon' deve ter no máximo 50 caracteres.");

            if (string.IsNullOrWhiteSpace(cartaPokemon.Raridade))
                throw new ValidationException("O campo 'Raridade' é obrigatório.");

            if (cartaPokemon.Raridade?.Length > 100)
                throw new ValidationException("O campo 'Raridade' deve ter no máximo 50 caracteres.");

            if (string.IsNullOrWhiteSpace(cartaPokemon.Tipo))
                throw new ValidationException("O campo 'Tipo' é obrigatório.");

            if (cartaPokemon.Tipo?.Length > 50)
                throw new ValidationException("O campo 'Tipo' deve ter no máximo 50 caracteres.");

            if (string.IsNullOrWhiteSpace(cartaPokemon.Estagio))
                throw new ValidationException("O campo 'Estágio' é obrigatório.");

            if (cartaPokemon.Estagio?.Length > 50)
                throw new ValidationException("O campo 'Estágio' deve ter no máximo 50 caracteres.");

            if (string.IsNullOrWhiteSpace(cartaPokemon.Booster))
                throw new ValidationException("O campo 'Booster' é obrigatório.");

            if (cartaPokemon.Booster?.Length > 100)
                throw new ValidationException("O campo 'Booster' deve ter no máximo 50 caracteres.");

            if (cartaPokemon.Imagem == null || cartaPokemon.Imagem.Length == 0)
                throw new ValidationException("O campo 'Imagem' é obrigatório.");

            if (string.IsNullOrWhiteSpace(cartaPokemon.Situacao))
                throw new ValidationException("O campo 'Situação' é obrigatório.");

            if (cartaPokemon.Situacao?.Length > 1)
                throw new ValidationException("O campo 'Situação' deve ter no máximo 1 caractere.");
        }
    }
}