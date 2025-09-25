using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.RepositoryInterface;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Infrastructure.Repositorys
{
    public class LivroRepository : GenericRepository<Livros>, ILivroRepository
    {
        public LivroRepository(MySQLContext context) : base(context)
        {
        }

        public List<Livros> ObterLivrosPorAutor(string autor)
        {
            if (string.IsNullOrEmpty(autor)) return null;

            return _context.Livros.Where(p => p.Autor.Contains(autor)).ToList();
        }
        public List<Livros> ObterLivrosPorTitulo(string titulo)
        {
            if (string.IsNullOrEmpty(titulo)) return null;

            return _context.Livros.Where(p => p.Titulo.Contains(titulo)).ToList();
        }
        public List<Livros> ObterLivrosPorDataLancamento(DateTime dataLancamento)
        {
            if (dataLancamento == null) return null;

            return _context.Livros.Where(p => p.DataLancamento.Equals(dataLancamento)).ToList();
        }
        public List<Livros> ObterLivrosPorPreco(decimal preco)
        {
            if (preco == 0) return null;
            return _context.Livros.Where(p => p.Preco.Equals(preco)).ToList();
        }

        public Livros Desativar(long id)
        {
            return AlterarStatus(id, false);
        }

        public Livros Ativar(long id)
        {
            return AlterarStatus(id, true);
        }

        private Livros AlterarStatus(long id, bool status)
        {
            var livro = _context.Livros.FirstOrDefault(p => p.Id.Equals(id));
            if (livro != null)
            {
                try
                {
                    livro.Ativo = status;
                    _context.Livros.Update(livro);
                    _context.SaveChanges();

                    return livro;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return null;
        }
    }
}