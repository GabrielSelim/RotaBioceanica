using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Domain.Service.Financas
{
    public class LancamentoValidationService : IEntityValidationService<Lancamento>
    {
        public void Validate(Lancamento lancamento)
        {
            if (lancamento.UsuarioId <= 0)
                throw new ValidationException("O campo 'Usuário' é obrigatório.");

            if (lancamento.CategoriaId <= 0)
                throw new ValidationException("O campo 'Categoria' é obrigatório.");

            if (string.IsNullOrWhiteSpace(lancamento.Descricao))
                throw new ValidationException("O campo 'Descrição' é obrigatório.");

            if (lancamento.Descricao.Length > 200)
                throw new ValidationException("O campo 'Descrição' deve ter no máximo 200 caracteres.");

            if (lancamento.Valor <= 0)
                throw new ValidationException("O campo 'Valor' deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(lancamento.TipoLancamento))
                throw new ValidationException("O campo 'Tipo de Lançamento' é obrigatório.");

            if (lancamento.TipoLancamento.Length > 50)
                throw new ValidationException("O campo 'Tipo de Lançamento' deve ter no máximo 50 caracteres.");

            if (string.IsNullOrWhiteSpace(lancamento.Situacao))
                throw new ValidationException("O campo 'Situação' é obrigatório.");

            if (lancamento.Situacao.Length > 20)
                throw new ValidationException("O campo 'Situação' deve ter no máximo 20 caracteres.");
        }
    }
}