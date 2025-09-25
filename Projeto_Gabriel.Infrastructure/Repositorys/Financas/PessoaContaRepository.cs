using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Infrastructure.Repositorys.Financas
{
    public class PessoaContaRepository : GenericRepository<PessoaConta>, IPessoaContaRepository
    {
        public PessoaContaRepository(MySQLContext context) : base(context)
        {
        }

        public PessoaConta? ObterPorIdSeguro(long id, long usuarioId)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            return _context.PessoaContas.FirstOrDefault(p => p.Id == id && p.UsuarioId == usuarioId);
        }

        public List<PessoaConta> ObterTodosSeguro(long usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            return _context.PessoaContas.Where(p => p.UsuarioId == usuarioId).ToList();
        }

        public List<PessoaConta> ObterPorNome(string nomePessoa, long usuarioId)
        {
            if (string.IsNullOrEmpty(nomePessoa))
                return new List<PessoaConta>();
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            return _context.PessoaContas
                .Where(p => p.NomePessoa.Contains(nomePessoa) && p.UsuarioId == usuarioId)
                .ToList();
        }
    }
}