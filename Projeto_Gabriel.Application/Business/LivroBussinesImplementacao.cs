using Projeto_Gabriel.Application.Converter.Implementacao;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Application.Hypermedia.Utils;
using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.RepositoryInterface;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Bussines.Implementacoes
{
    public class LivroBussinesImplementacao : ILivroBussines
    {

        private readonly ILivroRepository _livroRepository;
        private readonly LivroConverter _converter;

        public LivroBussinesImplementacao(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
            _converter = new LivroConverter();
        }

        public LivrosDbo ObterPorId(long id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido");

            var livro = _livroRepository.ObterPorId(id);

            if (livro == null)
                throw new NotFoundException("Livro não encontrado");

            return _converter.Parse(livro);
        }

        public List<LivrosDbo> ObterTodos()
        {
            var livros = _livroRepository.ObterTodos();

            if (livros == null || livros.Count == 0)
                throw new NotFoundException("Nenhum livro encontrado.");

            var parsedLivros = _converter.ParseList(livros);

            if (parsedLivros == null || parsedLivros.Count == 0)
                throw new NotFoundException("Nenhum livro encontrado.");

            return parsedLivros;
        }

        public LivrosDbo Criar(LivrosDbo livro)
        {
            try
            {
                var livroEntity = _converter.Parse(livro);

                if (livroEntity == null)
                    throw new ArgumentException("Erro ao converter o livro.");

                var livroCriado = _livroRepository.Criar(livroEntity);

                return _converter.Parse(livroCriado);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar o livro.", ex);
            }
        }

        public LivrosDbo Atualizar(LivrosDbo livro)
        {
            try
            {
                if (livro == null || livro.Id <= 0)
                    throw new ArgumentException("ID inválido");

                var livroEntity = _converter.Parse(livro);

                if (livroEntity == null)
                    throw new ArgumentException("Erro ao atualizar livro");

                livroEntity = _livroRepository.Atualizar(livroEntity);
                if (livroEntity == null)
                    throw new ArgumentException("Erro ao atualizar livro");

                LivrosDbo livroDbo = _converter.Parse(livroEntity);

                if (livroDbo == null)
                    throw new ArgumentException("Erro ao atualizar livro");

                return livroDbo;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Deletar(long id)
        {
            try
            {
                var livroEntity = _livroRepository.ObterPorId(id);

                if (livroEntity == null)
                    throw new NotFoundException("Livro não encontrado");

                _livroRepository.Deletar(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException($"Erro ao deletar livro: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Erro ao deletar livro: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar livro: " + ex.Message, ex);
            }
        }

        public List<LivrosDbo> ObterLivrosPorAutor(string autor)
        {
            var livros = _livroRepository.ObterLivrosPorAutor(autor);

            if (livros == null || livros.Count == 0)
            {
                throw new NotFoundException("Nenhum livro encontrado com o autor especificado.");
            }

            return _converter.ParseList(livros);
        }

        public List<LivrosDbo> ObterLivrosPorTitulo(string titulo)
        {
            var livros = _livroRepository.ObterLivrosPorTitulo(titulo);

            if (livros == null || livros.Count == 0)
            {
                throw new NotFoundException("Nenhum livro encontrado com o título especificado.");
            }

            return _converter.ParseList(livros);
        }

        public List<LivrosDbo> ObterLivrosPorDataLancamento(DateTime dataLancamento)
        {
            var livros = _livroRepository.ObterLivrosPorDataLancamento(dataLancamento);

            if (livros.Count == 0)
            {
                throw new NotFoundException("Nenhum livro encontrado com a data de lançamento especificada.");
            }

            return _converter.ParseList(livros);
        }

        public List<LivrosDbo> ObterLivrosPorPreco(decimal preco)
        {
            var livros = _livroRepository.ObterLivrosPorPreco(preco);

            if (livros.Count == 0)
            {
                throw new NotFoundException("Nenhum livro encontrado com o preço especificado.");
            }

            return _converter.ParseList(livros);
        }

        public LivrosDbo Desativar(long id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido");

            var livroEntity = _livroRepository.Desativar(id);


            if (livroEntity == null)
                throw new NotFoundException("Pessoa não encontrada");

            return _converter.Parse(livroEntity);
        }

        public LivrosDbo Ativar(long id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido");
            var livroEntity = _livroRepository.Ativar(id);

            if (livroEntity == null)
                throw new NotFoundException("Pessoa não encontrada");

            return _converter.Parse(livroEntity);
        }

        public PagedSearchDbo<LivrosDbo> pagedSearchDbo(string direcaoOrdenacao, int tamanhoPagina, int paginaAtual)
        {
            if (tamanhoPagina <= 0)
                throw new ArgumentException("O tamanho da página deve ser maior que zero.");

            if (paginaAtual <= 0)
                throw new ArgumentException("A página atual deve ser maior que zero.");

            if (string.IsNullOrEmpty(direcaoOrdenacao))
                direcaoOrdenacao = "asc";

            if (direcaoOrdenacao != "asc" && direcaoOrdenacao != "desc")
                throw new ArgumentException("Direção de ordenação deve ser 'asc' ou 'desc'");

            var resultado = _livroRepository.ObterComPaginacao(paginaAtual, tamanhoPagina,  direcaoOrdenacao);

            if (resultado == null || resultado.Count == 0)
                throw new NotFoundException("Nenhum livro encontrado.");

            var totalResults = _livroRepository.GetCount();

            return new PagedSearchDbo<LivrosDbo>
            {
                CurrentPage = paginaAtual,
                PageSize = tamanhoPagina,
                TotalResults = totalResults,
                SortDirections = direcaoOrdenacao,
                List = _converter.ParseList(resultado)
            };
        }
    }
}
