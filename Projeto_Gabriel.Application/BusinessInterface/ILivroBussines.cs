using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Application.Hypermedia.Utils;

namespace Projeto_Gabriel.Bussines
{
    public interface ILivroBussines
    {
        LivrosDbo Criar(LivrosDbo livro);

        LivrosDbo Atualizar(LivrosDbo livro);

        List<LivrosDbo> ObterTodos();

        LivrosDbo ObterPorId(long id);

        PagedSearchDbo<LivrosDbo> pagedSearchDbo(string direcaoOrdenacao, int tamanhoPagina, int paginaAtual);

        List<LivrosDbo> ObterLivrosPorAutor(string autor);

        List<LivrosDbo> ObterLivrosPorTitulo(string titulo);

        List<LivrosDbo> ObterLivrosPorDataLancamento(DateTime dataLancamento);

        List<LivrosDbo> ObterLivrosPorPreco(decimal preco);

        LivrosDbo Desativar(long id);

        LivrosDbo Ativar(long id);

        void Deletar(long id);
    }
}