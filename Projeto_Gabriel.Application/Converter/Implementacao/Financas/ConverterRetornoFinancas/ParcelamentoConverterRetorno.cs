using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoDbo;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas
{
    public class ParcelamentoConverterRetorno : IParser<ParcelamentoDbo, RetornoParcelamentoDbo>, IParser<RetornoParcelamentoDbo, ParcelamentoDbo>
    {
        public RetornoParcelamentoDbo Parse(ParcelamentoDbo origem)
        {
            if (origem == null) return null;
            return new RetornoParcelamentoDbo
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                ValorTotal = origem.ValorTotal,
                NumeroParcelas = origem.NumeroParcelas,
                DataPrimeiraParcela = origem.DataPrimeiraParcela,
                IntervaloParcelas = origem.IntervaloParcelas,
                CartaoId = origem.CartaoId,
                PessoaContaId = origem.PessoaContaId,
                Situacao = origem.Situacao
            };
        }

        public ParcelamentoDbo Parse(RetornoParcelamentoDbo origem)
        {
            if (origem == null) return null;
            return new ParcelamentoDbo
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                ValorTotal = origem.ValorTotal,
                NumeroParcelas = origem.NumeroParcelas,
                DataPrimeiraParcela = origem.DataPrimeiraParcela,
                IntervaloParcelas = origem.IntervaloParcelas,
                CartaoId = origem.CartaoId,
                PessoaContaId = origem.PessoaContaId,
                Situacao = origem.Situacao
            };
        }

        public List<RetornoParcelamentoDbo> ParseList(List<ParcelamentoDbo> origem)
        {
            if (origem == null || !origem.Any()) return new List<RetornoParcelamentoDbo>();
            return origem.Select(Parse).ToList();
        }

        public List<ParcelamentoDbo> ParseList(List<RetornoParcelamentoDbo> origem)
        {
            if (origem == null || !origem.Any()) return new List<ParcelamentoDbo>();
            return origem.Select(Parse).ToList();
        }
    }
}