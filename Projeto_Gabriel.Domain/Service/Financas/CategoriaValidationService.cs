using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Domain.Service.Financas
{
    public class CategoriaValidationService : IEntityValidationService<Categoria>
    {
        public void Validate(Categoria categoria)
        {
            if (categoria.UsuarioId <= 0)
                throw new ValidationException("O campo 'Usuário' é obrigatório.");

            if (string.IsNullOrWhiteSpace(categoria.NomeCategoria))
                throw new ValidationException("O campo 'Nome da Categoria' é obrigatório.");

            if (categoria.NomeCategoria.Length > 100)
                throw new ValidationException("O campo 'Nome da Categoria' deve ter no máximo 100 caracteres.");

            if (string.IsNullOrWhiteSpace(categoria.TipoCategoria))
                throw new ValidationException("O campo 'Tipo da Categoria' é obrigatório.");

            if (categoria.TipoCategoria.Length > 50)
                throw new ValidationException("O campo 'Tipo da Categoria' deve ter no máximo 50 caracteres.");
        }
    }
}