using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Infrastructure.Services.Validation
{
    public class LivrosValidationService : IEntityValidationService<Livros>
    {
        public void Validate(Livros livro)
        {
            if (string.IsNullOrWhiteSpace(livro.Autor))
                throw new ValidationException("O campo 'Autor' é obrigatório.");

            if (livro.Autor?.Length > 100)
                throw new ValidationException("O campo 'Autor' deve ter no máximo 50 caracteres.");

            if (string.IsNullOrWhiteSpace(livro.Titulo))
                throw new ValidationException("O campo 'Título' é obrigatório.");

            if (livro.Titulo?.Length > 100)
                throw new ValidationException("O campo 'Título' deve ter no máximo 50 caracteres.");

            if (livro.Preco < 0 || livro.Preco > 9999.99m)
                throw new ValidationException("O campo 'Preço' deve estar entre 0 e 9999.99.");
        }
    }
}