using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface.Financas
{
    public interface IPessoaContaRepository : IRepository<PessoaConta>
    {
        PessoaConta? ObterPorIdSeguro(long id, long usuarioId);

        List<PessoaConta> ObterTodosSeguro(long usuarioId);

        List<PessoaConta> ObterPorNome(string nomePessoa, long usuarioId);
    }
}