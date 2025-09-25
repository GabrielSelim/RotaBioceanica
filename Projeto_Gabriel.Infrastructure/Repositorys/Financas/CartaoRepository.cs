using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Infrastructure.Repositorys.Financas
{
    public class CartaoRepository : GenericRepository<Cartao>, ICartaoRepository
    {
        public CartaoRepository(MySQLContext context) : base(context)
        {
        }

        public Cartao? ObterPorIdSeguro(long id, long usuarioId)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            return _context.Cartoes.FirstOrDefault(c => c.Id == id && c.UsuarioId == usuarioId);
        }

        public List<Cartao> ObterTodosSeguro(long usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            return _context.Cartoes.Where(c => c.UsuarioId == usuarioId).ToList();
        }

        public List<Cartao> ObterCartoesPorNomeUsuario(string nomeUsuario, long usuarioId)
        {
            if (string.IsNullOrEmpty(nomeUsuario))
                return new List<Cartao>();
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            return _context.Cartoes
                .Where(c => c.NomeUsuario.Contains(nomeUsuario) && c.UsuarioId == usuarioId)
                .ToList();
        }

        public List<Cartao> ObterCartoesPorNomeBanco(string nomeBanco, long usuarioId)
        {
            if (string.IsNullOrEmpty(nomeBanco))
                return new List<Cartao>();
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            return _context.Cartoes
                .Where(c => c.NomeBanco.Contains(nomeBanco) && c.UsuarioId == usuarioId)
                .ToList();
        }
    }
}
