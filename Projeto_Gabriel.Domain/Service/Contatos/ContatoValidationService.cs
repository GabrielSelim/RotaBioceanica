using Projeto_Gabriel.Domain.Entity.Contatos;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Domain.Service.Contatos
{
    public class ContatoValidationService : IEntityValidationService<Contato>
    {
        public void Validate(Contato contato)
        {
            if (string.IsNullOrWhiteSpace(contato.Nome))
                throw new ValidationException("O campo 'Nome' é obrigatório.");

            if (contato.Nome?.Length > 100)
                throw new ValidationException("O campo 'Nome' deve ter no máximo 100 caracteres.");

            if (string.IsNullOrWhiteSpace(contato.Email))
                throw new ValidationException("O campo 'Email' é obrigatório.");

            if (contato.Email?.Length > 100)
                throw new ValidationException("O campo 'Email' deve ter no máximo 100 caracteres.");

            if (string.IsNullOrWhiteSpace(contato.Mensagem))
                throw new ValidationException("O campo 'Mensagem' é obrigatório.");

            if (contato.Mensagem?.Length > 1000)
                throw new ValidationException("O campo 'Mensagem' deve ter no máximo 1000 caracteres.");
        }
    }
}