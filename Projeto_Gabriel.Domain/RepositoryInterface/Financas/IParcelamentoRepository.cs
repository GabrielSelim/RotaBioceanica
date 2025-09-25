using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface.Financas
{
    public interface IParcelamentoRepository : IRepository<Parcelamento>
    {
        Parcelamento ObterPorIdSeguro(long id, long usuarioId);

        List<Parcelamento> ObterTodosPorUsuario(long usuarioId);

        Parcelamento AtualizarParcelamentoComCascata(Parcelamento parcelamento, long usuarioId);

        List<ParcelamentoMensal> ObterParcelamentosMensaisPorParcelamentoId(long parcelamentoId, long usuarioId);

        List<Parcelamento> ObterPorPessoaContaId(long pessoaContaId, long usuarioId);

        List<Parcelamento> ObterPorCartaoId(long cartaoId, long usuarioId);

        List<Parcelamento> ObterPorPessoaContaIdECartaoId(long pessoaContaId, long cartaoId, long usuarioId);

        List<Parcelamento> ObterPorSituacao(string situacao, long usuarioId);

        Parcelamento CriarComCascata(Parcelamento parcelamento);

        void DeletarEmCascata(long parcelamentoId, long usuarioId);
    }
}