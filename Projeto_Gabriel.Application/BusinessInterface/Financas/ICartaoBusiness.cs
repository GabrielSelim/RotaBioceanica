using Projeto_Gabriel.Application.Dto.Financas.CartaoDbo;

namespace Projeto_Gabriel.Application.BusinessInterface.Financas
{
    public interface ICartaoBusiness
    {
        RetornoCartaoDbo ObterPorId(long id, long usuarioId);

        List<RetornoCartaoDbo> ObterTodos(long usuarioId);

        RetornoCartaoDbo Criar(CriarCartaoDbo cartao, long usuarioId);

        List<RetornoCartaoDbo> ObterCartoesPorNomeUsuario(string nomeUsuario, long usuarioId);

        List<RetornoCartaoDbo> ObterCartoesPorNomeBanco(string nomeBanco, long usuarioId);

        RetornoCartaoDbo AtualizarCartao(AtualizarCartaoDbo cartao, long usuarioId);

        void DeletarCartao(long id, long usuarioId);
    }
}