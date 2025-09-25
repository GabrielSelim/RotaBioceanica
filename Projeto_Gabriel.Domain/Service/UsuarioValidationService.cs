using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Infrastructure.Services.Validation
{
    public class UsuarioValidationService : IEntityValidationService<Usuario>
    {
        public void Validate(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.NomeUsuario))
                throw new ValidationException("O campo 'Nome de Usuário' é obrigatório.");

            if (string.IsNullOrWhiteSpace(usuario.NomeCompleto))
                throw new ValidationException("O campo 'Nome Completo' é obrigatório.");

            if (string.IsNullOrWhiteSpace(usuario.Senha))
                throw new ValidationException("O campo 'Senha' é obrigatório.");

            if (usuario.NomeUsuario?.Length > 50)
                throw new ValidationException("O campo 'NomeUsuario' deve ter no máximo 50 caracteres.");

            if (usuario.NomeCompleto?.Length > 100)
                throw new ValidationException("O campo 'NomeCompleto' deve ter no máximo 50 caracteres.");
        }
    }
}