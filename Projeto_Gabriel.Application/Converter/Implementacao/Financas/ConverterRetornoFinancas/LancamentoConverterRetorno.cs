using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas
{
    public class LancamentoConverterRetorno : IParser<LancamentoDbo, RetornoLancamentoDbo>, IParser<RetornoLancamentoDbo, LancamentoDbo>
    {
        public RetornoLancamentoDbo Parse(LancamentoDbo origem)
        {
            if (origem == null) return null;
            return new RetornoLancamentoDbo
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                Valor = origem.Valor,
                DataLancamento = origem.DataLancamento,
                TipoLancamento = origem.TipoLancamento,
                CategoriaId = origem.CategoriaId,
                ParcelamentoMensalId = origem.ParcelamentoMensalId,
                Situacao = origem.Situacao
            };
        }

        public LancamentoDbo Parse(RetornoLancamentoDbo origem)
        {
            if (origem == null) return null;
            return new LancamentoDbo
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                Valor = origem.Valor,
                TipoLancamento = origem.TipoLancamento,
                DataLancamento = origem.DataLancamento,
                CategoriaId = origem.CategoriaId,
                ParcelamentoMensalId = origem.ParcelamentoMensalId,
                Situacao = origem.Situacao
            };
        }

        public List<RetornoLancamentoDbo> ParseList(List<LancamentoDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }

        public List<LancamentoDbo> ParseList(List<RetornoLancamentoDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}