using Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoMensalDbo;

namespace Projeto_Gabriel.Application.BusinessInterface.Financas
{
    public interface IParcelamentoMensalBusiness
    {
        RetornoParcelamentoMensalDbo ObterPorId(long id, long usuarioId);

        List<RetornoParcelamentoMensalDbo> ObterTodos(long usuarioId);

        List<RetornoParcelamentoMensalDbo> ObterPorParcelamentoId(long parcelamentoId, long usuarioId);

        List<RetornoParcelamentoMensalDbo> ObterPorSituacao(string situacao, long usuarioId);

        List<RetornoParcelamentoMensalDbo> ObterPorPessoaContaId(long pessoaContaId, long usuarioId);

        List<RetornoParcelamentoMensalDbo> ObterPorCartaoId(long cartaoId, long usuarioId);

        void MarcarComoPago(MarcarComoPagoRequest request, long usuarioId);

        void MarcarComoInativo(long id, long usuarioId);
    }
}