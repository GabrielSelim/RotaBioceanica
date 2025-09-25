using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface
{
    public interface IPessoaRepository : IRepository<Pessoa>
    {
        Pessoa Desativar(long id);

        Pessoa Ativar(long id);

        List<Pessoa> ObterPessoasPorNome(string nome);

        List<Pessoa> ObterPessoasPorEndereco(string endereco);
    }
}