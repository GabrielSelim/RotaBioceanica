using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas
{
    public class LancamentoConverter : IParser<LancamentoDbo, Lancamento>, IParser<Lancamento, LancamentoDbo>
    {
        public Lancamento Parse(LancamentoDbo origem)
        {
            if (origem == null) return null;
            return new Lancamento
            {
                Id = origem.Id,
                UsuarioId = origem.UsuarioId,
                DataLancamento = origem.DataLancamento,
                Descricao = origem.Descricao,
                Valor = origem.Valor,
                TipoLancamento = origem.TipoLancamento,
                CategoriaId = origem.CategoriaId,
                ParcelamentoMensalId = origem.ParcelamentoMensalId,
                Situacao = origem.Situacao
            };
        }

        public LancamentoDbo Parse(Lancamento origem)
        {
            if (origem == null) return null;
            return new LancamentoDbo
            {
                Id = origem.Id,
                UsuarioId = origem.UsuarioId,
                DataLancamento = origem.DataLancamento,
                Descricao = origem.Descricao,
                Valor = origem.Valor,
                TipoLancamento = origem.TipoLancamento,
                CategoriaId = origem.CategoriaId,
                ParcelamentoMensalId = origem.ParcelamentoMensalId,
                Situacao = origem.Situacao
            };
        }

        public List<Lancamento> ParseList(List<LancamentoDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }

        public List<LancamentoDbo> ParseList(List<Lancamento> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}