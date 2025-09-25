using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Domain.Service.Financas
{
    public class ParcelamentoValidationService : IEntityValidationService<Parcelamento>
    {
        public void Validate(Parcelamento parcelamento)
        {
            if (parcelamento.UsuarioId <= 0)
                throw new ValidationException("O campo 'Usuário' é obrigatório.");

            if (string.IsNullOrWhiteSpace(parcelamento.Descricao))
                throw new ValidationException("O campo 'Descrição' é obrigatório.");

            if (parcelamento.Descricao.Length > 200)
                throw new ValidationException("O campo 'Descrição' deve ter no máximo 200 caracteres.");

            if (parcelamento.ValorTotal <= 0)
                throw new ValidationException("O campo 'Valor Total' deve ser maior que zero.");

            if (parcelamento.NumeroParcelas <= 0)
                throw new ValidationException("O campo 'Número de Parcelas' deve ser maior que zero.");

            if (parcelamento.IntervaloParcelas <= 0)
                throw new ValidationException("O campo 'Intervalo de Parcelas' deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(parcelamento.Situacao))
                throw new ValidationException("O campo 'Situação' é obrigatório.");

            if (parcelamento.Situacao.Length > 20)
                throw new ValidationException("O campo 'Situação' deve ter no máximo 20 caracteres.");
        }
    }
}