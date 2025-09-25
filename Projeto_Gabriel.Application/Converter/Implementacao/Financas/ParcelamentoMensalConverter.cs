using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoMensalDbo;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas
{
    public class ParcelamentoMensalConverter : IParser<ParcelamentoMensalDbo, ParcelamentoMensal>, IParser<ParcelamentoMensal, ParcelamentoMensalDbo>
    {
        public ParcelamentoMensal Parse(ParcelamentoMensalDbo origem)
        {
            if (origem == null) return null;
            return new ParcelamentoMensal
            {
                Id = origem.Id,
                ParcelamentoId = origem.ParcelamentoId,
                NumeroParcela = origem.NumeroParcela,
                DataVencimento = origem.DataVencimento,
                DataPagamento = origem.DataPagamento,
                ValorParcela = origem.ValorParcela,
                ValorPago = origem.ValorPago,
                CartaoId = origem.CartaoId,
                PessoaContaId = origem.PessoaContaId,
                Situacao = origem.Situacao
            };
        }

        public ParcelamentoMensalDbo Parse(ParcelamentoMensal origem)
        {
            if (origem == null) return null;
            return new ParcelamentoMensalDbo
            {
                Id = origem.Id,
                ParcelamentoId = origem.ParcelamentoId,
                NumeroParcela = origem.NumeroParcela,
                DataVencimento = origem.DataVencimento,
                DataPagamento = origem.DataPagamento,
                ValorParcela = origem.ValorParcela,
                ValorPago = origem.ValorPago,
                CartaoId = origem.CartaoId,
                PessoaContaId = origem.PessoaContaId,
                Situacao = origem.Situacao
            };
        }

        public List<ParcelamentoMensal> ParseList(List<ParcelamentoMensalDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }

        public List<ParcelamentoMensalDbo> ParseList(List<ParcelamentoMensal> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}