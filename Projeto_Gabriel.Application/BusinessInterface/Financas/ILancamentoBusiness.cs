using Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo;

namespace Projeto_Gabriel.Application.BusinessInterface.Financas
{
    public interface ILancamentoBusiness
    {
        RetornoLancamentoDbo ObterPorId(long id, long usuarioId);

        List<RetornoLancamentoDbo> ObterTodos(long usuarioId);

        RetornoLancamentoDbo Criar(CriarLancamentoDbo lancamento, long usuarioId);

        List<RetornoLancamentoDbo> ObterPorCategoriaId(long categoriaId, long usuarioId);

        List<RetornoLancamentoDbo> ObterPorParcelamentoMensalId(long parcelamentoMensalId, long usuarioId);

        List<RetornoLancamentoDbo> ObterPorPeriodo(long usuarioId, DateTime dataInicio, DateTime dataFim);

        List<RetornoLancamentoDbo> ObterPorSituacao(string situacao, long usuarioId);

        void MarcarComoPago(MarcarComoPagoRequest request, long usuarioId);

        RetornoLancamentoDbo Atualizar(AtualizarLancamentoDbo atualizar, long usuarioId);

        void Deletar(long id, long usuarioId);
    }
}