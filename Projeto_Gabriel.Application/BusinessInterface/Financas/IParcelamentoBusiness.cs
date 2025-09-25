using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoDbo;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoMensalDbo;

namespace Projeto_Gabriel.Application.BusinessInterface.Financas
{
    public interface IParcelamentoBusiness
    {
        RetornoParcelamentoDbo ObterPorId(long id, long usuarioId);

        List<RetornoParcelamentoDbo> ObterTodos(long usuarioId);

        RetornoParcelamentoDbo Criar(CriarParcelamentoDbo parcelamento, long usuarioId);

        RetornoParcelamentoDbo Atualizar(AtualizarParcelamentoDbo parcelamento, long usuarioId);

        List<RetornoParcelamentoMensalDbo> ObterParcelamentosMensaisPorParcelamentoId(long parcelamentoId, long usuarioId);

        List<RetornoParcelamentoDbo> ObterPorPessoaContaId(long pessoaContaId, long usuarioId);

        List<RetornoParcelamentoDbo> ObterPorCartaoId(long cartaoId, long usuarioId);

        List<RetornoParcelamentoDbo> ObterPorPessoaContaIdECartaoId(long pessoaContaId, long cartaoId, long usuarioId);

        List<RetornoParcelamentoDbo> ObterPorSituacao(string situacao, long usuarioId);

        void DeletarParcelamento(long id, long usuarioId);
    }
}