using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface.Financas
{
    public interface IParcelamentoMensalRepository : IRepository<ParcelamentoMensal>
    {
        List<ParcelamentoMensal> ObterTodosSeguro(long usuarioId);

        ParcelamentoMensal? ObterPorIdSeguro(long id, long usuarioId);

        List<ParcelamentoMensal> ObterPorParcelamentoId(long parcelamentoId, long usuarioId);

        List<ParcelamentoMensal> ObterPorSituacao(string situacao, long usuarioId);

        List<ParcelamentoMensal> ObterPorPessoaContaId(long pessoaContaId, long usuarioId);

        List<ParcelamentoMensal> ObterPorCartaoId(long cartaoId, long usuarioId);

        void MarcarComoPago(long id, decimal valorPago, DateTime dataPagamento, long usuarioId);

        void MarcarComoInativo(long id, long usuarioId);
    }
}