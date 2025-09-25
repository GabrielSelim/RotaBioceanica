using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface.Financas
{
    public interface ILancamentoRepository : IRepository<Lancamento>
    {
        List<Lancamento> ObterTodosSeguro(long usuarioId);

        Lancamento? ObterPorIdSeguro(long id, long usuarioId);

        List<Lancamento> ObterPorUsuarioId(int usuarioId);

        List<Lancamento> ObterPorCategoriaId(int categoriaId, long usuarioId);

        List<Lancamento> ObterPorParcelamentoMensalId(int parcelamentoMensalId, long usuarioId);

        List<Lancamento> ObterPorPeriodo(long usuarioId, DateTime dataInicio, DateTime dataFim);

        List<Lancamento> ObterPorSituacao(string situacao, long usuarioId);

        void MarcarComoPago(long lancamentoId, decimal valorPago, DateTime dataPagamento, long usuarioId);
    }
}