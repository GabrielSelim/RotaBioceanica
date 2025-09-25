using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Domain.Service.Financas
{
    public class CartaoValidationService : IEntityValidationService<Cartao>
    {
        public void Validate(Cartao cartao)
        {
            if (string.IsNullOrWhiteSpace(cartao.NomeUsuario))
                throw new ValidationException("O campo 'Nome do Usuário' é obrigatório.");

            if (cartao.NomeUsuario.Length > 100)
                throw new ValidationException("O campo 'Nome do Usuário' deve ter no máximo 100 caracteres.");

            if (string.IsNullOrWhiteSpace(cartao.NomeBanco))
                throw new ValidationException("O campo 'Nome do Banco' é obrigatório.");

            if (cartao.NomeBanco.Length > 100)
                throw new ValidationException("O campo 'Nome do Banco' deve ter no máximo 100 caracteres.");
        }
    }
}