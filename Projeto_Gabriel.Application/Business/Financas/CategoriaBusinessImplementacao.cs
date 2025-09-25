using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas;
using Projeto_Gabriel.Application.Dto.Financas.CategoriaDbo;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;

namespace Projeto_Gabriel.Application.Business.Financas
{
    public class CategoriaBusinessImplementacao : ICategoriaBusiness
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly CategoriaConverter _converter;
        private readonly CategoriaConverterRetorno _converterRetorno;

        public CategoriaBusinessImplementacao(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
            _converter = new CategoriaConverter();
            _converterRetorno = new CategoriaConverterRetorno();
        }

        public RetornoCategoriaDbo ObterPorId(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var categoria = _categoriaRepository.ObterPorIdSeguro(id, usuarioId);

                if (categoria == null)
                    throw new NotFoundException("Categoria não encontrada");

                var categoriaDbo = _converter.Parse(categoria);
                return _converterRetorno.Parse(categoriaDbo);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar categoria por ID.", ex);
            }
        }

        public List<RetornoCategoriaDbo> ObterTodos(long usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var categorias = _categoriaRepository.ObterTodosSeguro(usuarioId);

                if (categorias == null || categorias.Count == 0)
                {
                    var categoriasPadrao = new List<Categoria>
                    {
                        new Categoria { UsuarioId = usuarioId, NomeCategoria = "Salário", TipoCategoria = "Receita" },
                        new Categoria { UsuarioId = usuarioId, NomeCategoria = "Receita", TipoCategoria = "Receita" },
                        new Categoria { UsuarioId = usuarioId, NomeCategoria = "Alimentação", TipoCategoria = "Despesa" },
                        new Categoria { UsuarioId = usuarioId, NomeCategoria = "Uber", TipoCategoria = "Despesa" },
                        new Categoria { UsuarioId = usuarioId, NomeCategoria = "Gasolina", TipoCategoria = "Despesa" },
                        new Categoria { UsuarioId = usuarioId, NomeCategoria = "FreeLance", TipoCategoria = "Receita" }
                    };

                    foreach (var categoria in categoriasPadrao)
                    {
                        _categoriaRepository.Criar(categoria);
                    }

                    categorias = _categoriaRepository.ObterTodosSeguro(usuarioId);
                }

                var parsedCategorias = _converter.ParseList(categorias);

                if (parsedCategorias == null || parsedCategorias.Count == 0)
                    throw new NotFoundException("Nenhuma categoria encontrada.");

                return _converterRetorno.ParseList(parsedCategorias);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar todas as categorias.", ex);
            }
        }

        public RetornoCategoriaDbo Criar(CriarCategoriaDbo categoria, long usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                CategoriaDbo categoriaDbo = new CategoriaDbo
                {
                    UsuarioId = usuarioId,
                    NomeCategoria = categoria.NomeCategoria,
                    TipoCategoria = categoria.TipoCategoria
                };

                var categoriaEntity = _converter.Parse(categoriaDbo);

                if (categoriaEntity == null)
                    throw new ArgumentException("Erro ao converter a categoria.");

                var categoriaCriada = _categoriaRepository.Criar(categoriaEntity);

                var categoriaDboCriada = _converter.Parse(categoriaCriada);

                return _converterRetorno.Parse(categoriaDboCriada);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar a categoria.", ex);
            }
        }

        public List<RetornoCategoriaDbo> ObterCategoriasPorNome(string nomeCategoria, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeCategoria))
                    throw new ArgumentException("O nome da categoria não pode ser vazio.");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var categorias = _categoriaRepository.ObterCategoriasPorNome(nomeCategoria, usuarioId);

                if (categorias == null || categorias.Count == 0)
                    throw new NotFoundException("Nenhuma categoria encontrada com o nome especificado.");

                var parsedCategorias = _converter.ParseList(categorias);

                return _converterRetorno.ParseList(parsedCategorias);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar categorias por nome.", ex);
            }
        }

        public List<RetornoCategoriaDbo> ObterCategoriasPorTipo(string tipoCategoria, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tipoCategoria))
                    throw new ArgumentException("O tipo da categoria não pode ser vazio.");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var categorias = _categoriaRepository.ObterCategoriasPorTipo(tipoCategoria, usuarioId);

                if (categorias == null || categorias.Count == 0)
                    throw new NotFoundException("Nenhuma categoria encontrada com o tipo especificado.");

                var parsedCategorias = _converter.ParseList(categorias);

                return _converterRetorno.ParseList(parsedCategorias);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar categorias por tipo.", ex);
            }
        }

        public List<RetornoCategoriaDbo> ObterCategoriasPorUsuario(long usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("O ID do usuário deve ser maior que zero.");

                var categorias = _categoriaRepository.ObterTodosSeguro(usuarioId);

                if (categorias == null || categorias.Count == 0)
                    throw new NotFoundException("Nenhuma categoria encontrada para o usuário especificado.");

                var parsedCategorias = _converter.ParseList(categorias);

                return _converterRetorno.ParseList(parsedCategorias);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar categorias por usuário.", ex);
            }
        }

        public RetornoCategoriaDbo AtualizarCategoria(AtualizarCategoriaDbo categoria, long usuarioId)
        {
            try
            {
                if (categoria.Id <= 0)
                    throw new ArgumentException("ID inválido");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var categoriaExistente = _categoriaRepository.ObterPorIdSeguro(categoria.Id, usuarioId);

                if (categoriaExistente == null)
                    throw new NotFoundException("Cartão Não Encontrado.");

                categoriaExistente.NomeCategoria = categoria.NomeCategoria;
                categoriaExistente.TipoCategoria = categoria.TipoCategoria;
                var categoriaAtualizada = _categoriaRepository.Atualizar(categoriaExistente);
                var categoriaDboAtualiada = _converter.Parse(categoriaAtualizada);

                return _converterRetorno.Parse(categoriaDboAtualiada);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar a categoria.", ex);
            }
        }

        public void DeletarCategoria(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var categoriaExistente = _categoriaRepository.ObterPorIdSeguro(id, usuarioId);

                if (categoriaExistente == null)
                    throw new NotFoundException("Cartão Não Encontrado.");

                if (categoriaExistente == null)
                    throw new NotFoundException("Categoria não encontrado");

                _categoriaRepository.Deletar(categoriaExistente.Id);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar o categoria.", ex);
            }
        }
    }
}