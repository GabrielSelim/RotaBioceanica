using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface.Financas
{
    public interface  ICartaoRepository : IRepository<Cartao>
    {
        Cartao? ObterPorIdSeguro(long id, long usuarioId);

        List<Cartao> ObterTodosSeguro(long usuarioId);

        List<Cartao> ObterCartoesPorNomeUsuario(string nomeUsuario, long usuarioId);

        List<Cartao> ObterCartoesPorNomeBanco(string nomeBanco, long usuarioId);
    }
}