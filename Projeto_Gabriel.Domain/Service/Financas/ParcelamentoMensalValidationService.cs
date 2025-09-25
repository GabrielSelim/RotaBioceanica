using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.ServiceInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Domain.Service.Financas
{
    public class ParcelamentoMensalValidationService : IEntityValidationService<ParcelamentoMensal>
    {
        public void Validate(ParcelamentoMensal mensal)
        {
            if (mensal.ParcelamentoId <= 0)
                throw new ValidationException("O campo 'Parcelamento' é obrigatório.");

            if (mensal.NumeroParcela <= 0)
                throw new ValidationException("O campo 'Número da Parcela' deve ser maior que zero.");

            if (mensal.ValorParcela <= 0)
                throw new ValidationException("O campo 'Valor da Parcela' deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(mensal.Situacao))
                throw new ValidationException("O campo 'Situação' é obrigatório.");

            if (mensal.Situacao.Length > 20)
                throw new ValidationException("O campo 'Situação' deve ter no máximo 20 caracteres.");
        }
    }
}