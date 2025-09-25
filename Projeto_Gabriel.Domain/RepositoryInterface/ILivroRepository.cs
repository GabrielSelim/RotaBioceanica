using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface
{
    public interface ILivroRepository : IRepository<Livros>
    {
        Livros Desativar(long id);

        Livros Ativar(long id);

        List<Livros> ObterLivrosPorAutor(string autor);

        List<Livros> ObterLivrosPorTitulo(string titulo);

        List<Livros> ObterLivrosPorDataLancamento(DateTime dataLancamento);

        List<Livros> ObterLivrosPorPreco(decimal preco);
    }
}