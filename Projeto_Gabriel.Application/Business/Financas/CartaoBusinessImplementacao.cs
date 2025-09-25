using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas;
using Projeto_Gabriel.Application.Dto.Financas.CartaoDbo;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;

namespace Projeto_Gabriel.Application.Business.Financas
{
    public class CartaoBusinessImplementacao : ICartaoBusiness
    {
        private readonly ICartaoRepository _cartaoRepository;
        private readonly CartaoConverter _converter;
        private readonly CartaoConverterRetorno _converterRetorno;

        public CartaoBusinessImplementacao(ICartaoRepository cartaoRepository)
        {
            _cartaoRepository = cartaoRepository;
            _converter = new CartaoConverter();
            _converterRetorno = new CartaoConverterRetorno();
        }

        public RetornoCartaoDbo ObterPorId(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var cartao = _cartaoRepository.ObterPorIdSeguro(id, usuarioId);

                if (cartao == null)
                    throw new NotFoundException("Cartão não encontrado");

                var cartaoDbo = _converter.Parse(cartao);

                return _converterRetorno.Parse(cartaoDbo);
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
                throw new Exception("Erro ao buscar cartão por ID.", ex);
            }
        }

        public List<RetornoCartaoDbo> ObterTodos(long usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var cartoes = _cartaoRepository.ObterTodosSeguro(usuarioId);

                if (cartoes == null || cartoes.Count == 0)
                    throw new NotFoundException("Nenhum cartão encontrado.");

                var parsedCartoes = _converter.ParseList(cartoes);

                if (parsedCartoes == null || parsedCartoes.Count == 0)
                    throw new NotFoundException("Nenhum cartão encontrado.");

                return _converterRetorno.ParseList(parsedCartoes);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar todos os cartões.", ex);
            }
        }

        public RetornoCartaoDbo Criar(CriarCartaoDbo cartao, long usuarioId)
        {
            try
            {
                CartaoDbo cartaoDbo = new CartaoDbo()
                {
                    UsuarioId = usuarioId,
                    NomeUsuario = cartao.NomeUsuario,
                    NomeBanco = cartao.NomeBanco
                };

                var cartaoEntity = _converter.Parse(cartaoDbo);

                if (cartaoEntity == null)
                    throw new ArgumentException("Erro ao converter o cartão.");

                var cartaoCriado = _cartaoRepository.Criar(cartaoEntity);

                var cartaoDboCriado = _converter.Parse(cartaoCriado);

                return _converterRetorno.Parse(cartaoDboCriado);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar o cartão.", ex);
            }
        }

        public List<RetornoCartaoDbo> ObterCartoesPorNomeUsuario(string nomeUsuario, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeUsuario))
                    throw new ArgumentException("O nome do usuário não pode ser vazio.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var cartoes = _cartaoRepository.ObterCartoesPorNomeUsuario(nomeUsuario, usuarioId);

                if (cartoes == null || cartoes.Count == 0)
                    throw new NotFoundException("Nenhum cartão encontrado com o nome de usuário especificado.");

                var parsedCartoes = _converter.ParseList(cartoes);

                return _converterRetorno.ParseList(parsedCartoes);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar cartões por nome de usuário.", ex);
            }
        }

        public List<RetornoCartaoDbo> ObterCartoesPorNomeBanco(string nomeBanco, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeBanco))
                    throw new ArgumentException("O nome do banco não pode ser vazio.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var cartoes = _cartaoRepository.ObterCartoesPorNomeBanco(nomeBanco, usuarioId);

                if (cartoes == null || cartoes.Count == 0)
                    throw new NotFoundException("Nenhum cartão encontrado com o nome do banco especificado.");

                var parsedCartoes = _converter.ParseList(cartoes);

                return _converterRetorno.ParseList(parsedCartoes);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar cartões por nome do banco.", ex);
            }
        }

        public RetornoCartaoDbo AtualizarCartao(AtualizarCartaoDbo cartao, long usuarioId)
        {
            try
            {
                if (cartao.Id <= 0)
                    throw new ArgumentException("ID inválido");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var cartaoExistente = _cartaoRepository.ObterPorIdSeguro(cartao.Id, usuarioId);

                if (cartaoExistente == null)
                    throw new NotFoundException("Cartão não encontrado");

                cartaoExistente.NomeUsuario = cartao.NomeUsuario;
                cartaoExistente.NomeBanco = cartao.NomeBanco;

                var cartaoAtualizado = _cartaoRepository.Atualizar(cartaoExistente);

                var cartaoDboAtualizado = _converter.Parse(cartaoAtualizado);

                return _converterRetorno.Parse(cartaoDboAtualizado);
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
                throw new Exception("Erro ao atualizar o cartão.", ex);
            }
        }

        public void DeletarCartao(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var cartaoExistente = _cartaoRepository.ObterPorIdSeguro(id, usuarioId);

                if (cartaoExistente == null)
                    throw new NotFoundException("Cartão não encontrado");

                _cartaoRepository.Deletar(cartaoExistente.Id);
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
                throw new Exception("Erro ao deletar o cartão.", ex);
            }
        }
    }
}