using Projeto_Gabriel.Domain.Entity.Contatos;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface.Contatos
{
    public interface IContatoRepository : IRepository<Contato>
    {
        Task SalvarContatoAsync(Contato contato);
    }
}