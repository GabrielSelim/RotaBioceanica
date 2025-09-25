using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Domain.Service.Financas
{
    public class PessoaContaValidationService : IEntityValidationService<PessoaConta>
    {
        public void Validate(PessoaConta pessoaConta)
        {
            if (string.IsNullOrWhiteSpace(pessoaConta.NomePessoa))
                throw new ValidationException("O campo 'Nome da Pessoa' é obrigatório.");

            if (pessoaConta.NomePessoa.Length > 100)
                throw new ValidationException("O campo 'Nome da Pessoa' deve ter no máximo 100 caracteres.");
        }
    }
}