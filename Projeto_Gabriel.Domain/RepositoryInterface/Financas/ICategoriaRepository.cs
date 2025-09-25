using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface.Financas
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Categoria? ObterPorIdSeguro(long id, long usuarioId);

        List<Categoria> ObterTodosSeguro(long usuarioId);

        List<Categoria> ObterCategoriasPorNome(string nomeCategoria, long usuarioId);

        List<Categoria> ObterCategoriasPorTipo(string tipoCategoria, long usuarioId);
    }
}