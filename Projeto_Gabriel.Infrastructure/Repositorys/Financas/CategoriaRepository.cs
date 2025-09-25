using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Infrastructure.Repositorys.Financas
{
    public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(MySQLContext context) : base(context)
        {
        }
        public Categoria? ObterPorIdSeguro(long id, long usuarioId)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            return _context.Categorias.FirstOrDefault(c => c.Id == id && c.UsuarioId == usuarioId);
        }

        public List<Categoria> ObterTodosSeguro(long usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            return _context.Categorias.Where(c => c.UsuarioId == usuarioId).ToList();
        }

        public List<Categoria> ObterCategoriasPorNome(string nomeCategoria, long usuarioId)
        {
            if (nomeCategoria == null)
                throw new ArgumentNullException(nameof(nomeCategoria), "Nome da categoria não pode ser nulo.");

            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");
            
            if (string.IsNullOrEmpty(nomeCategoria)) return null;

            return _context.Categorias.Where(c => c.NomeCategoria.Contains(nomeCategoria) && c.UsuarioId == usuarioId).ToList();
            
        }

        public List<Categoria> ObterCategoriasPorTipo(string tipoCategoria, long usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            if (string.IsNullOrEmpty(tipoCategoria)) return null;

            return _context.Categorias.Where(c => c.TipoCategoria.Contains(tipoCategoria) && c.UsuarioId == usuarioId).ToList();
        }
    }
}