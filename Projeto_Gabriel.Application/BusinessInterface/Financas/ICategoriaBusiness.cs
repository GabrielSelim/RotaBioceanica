using Projeto_Gabriel.Application.Dto.Financas.CategoriaDbo;

namespace Projeto_Gabriel.Application.BusinessInterface.Financas
{
    public interface ICategoriaBusiness
    {
        RetornoCategoriaDbo ObterPorId(long id, long usuarioId);

        List<RetornoCategoriaDbo> ObterTodos(long usuarioId);

        RetornoCategoriaDbo Criar(CriarCategoriaDbo categoria, long usuarioId);

        List<RetornoCategoriaDbo> ObterCategoriasPorNome(string nomeCategoria, long usuarioId);

        List<RetornoCategoriaDbo> ObterCategoriasPorTipo(string tipoCategoria, long usuarioId);

        List<RetornoCategoriaDbo> ObterCategoriasPorUsuario(long usuarioId);

        RetornoCategoriaDbo AtualizarCategoria(AtualizarCategoriaDbo cartao, long usuarioId);

        void DeletarCategoria(long id, long usuarioId);
    }
}