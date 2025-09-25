using Projeto_Gabriel.Domain.Entity.Base;

namespace Projeto_Gabriel.Repository.Generic
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Criar(T item);

        T Atualizar(T item);

        List<T> ObterTodos();

        T ObterPorId(long id);

        void Deletar(long id);

        bool Existe(long id);

        List<T> ObterComPaginacao(int pageNumber, int pageSize, string sortDirection);

        int GetCount();
    }
}