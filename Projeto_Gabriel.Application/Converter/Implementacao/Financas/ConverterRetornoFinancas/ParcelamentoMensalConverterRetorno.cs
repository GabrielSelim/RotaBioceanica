using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoMensalDbo;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas
{
    public class ParcelamentoMensalConverterRetorno : IParser<ParcelamentoMensalDbo, RetornoParcelamentoMensalDbo>, IParser<RetornoParcelamentoMensalDbo, ParcelamentoMensalDbo>
    {
        public RetornoParcelamentoMensalDbo Parse(ParcelamentoMensalDbo origem)
        {
            if (origem == null) return null;
            return new RetornoParcelamentoMensalDbo
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

        public ParcelamentoMensalDbo Parse(RetornoParcelamentoMensalDbo origem)
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

        public List<RetornoParcelamentoMensalDbo> ParseList(List<ParcelamentoMensalDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }

        public List<ParcelamentoMensalDbo> ParseList(List<RetornoParcelamentoMensalDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();

        }
    }
}