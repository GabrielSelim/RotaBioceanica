using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Infrastructure.Services.Validation
{
    public class PessoaValidationService : IEntityValidationService<Pessoa>
    {
        public void Validate(Pessoa pessoa)
        {
            if (string.IsNullOrWhiteSpace(pessoa.PrimeiroNome))
                throw new ValidationException("O campo 'Primeiro Nome' é obrigatório.");

            if (pessoa.PrimeiroNome?.Length > 100)
                throw new ValidationException("O campo 'Primeiro Nome' deve ter no máximo 50 caracteres.");

            if (string.IsNullOrWhiteSpace(pessoa.Sobrenome))
                throw new ValidationException("O campo 'Sobrenome' é obrigatório.");

            if (pessoa.Sobrenome?.Length > 100)
                throw new ValidationException("O campo 'Sobrenome' deve ter no máximo 50 caracteres.");

            if (!string.IsNullOrWhiteSpace(pessoa.Endereco) && pessoa.Endereco.Length > 100)
                throw new ValidationException("O campo 'Endereço' deve ter no máximo 50 caracteres.");

            if (!string.IsNullOrWhiteSpace(pessoa.Sexo) && pessoa.Sexo.Length > 50)
                throw new ValidationException("O campo 'Sexo' deve ter no máximo 50 caracteres.");
        }
    }
}