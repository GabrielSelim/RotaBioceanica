using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoDbo;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoMensalDbo;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;

namespace Projeto_Gabriel.Application.Business.Financas
{
    public class ParcelamentoBusinessImplementacao : IParcelamentoBusiness
    {
        private readonly IParcelamentoRepository _parcelamentoRepository;
        private readonly ParcelamentoConverter _converter;
        private readonly ParcelamentoMensalConverter _mensalConverter;
        private readonly ParcelamentoConverterRetorno _converterRetorno;
        private readonly ParcelamentoMensalConverterRetorno _mensalConverterRetorno;
        public ParcelamentoBusinessImplementacao(IParcelamentoRepository parcelamentoRepository)
        {
            _parcelamentoRepository = parcelamentoRepository;
            _converter = new ParcelamentoConverter();
            _mensalConverter = new ParcelamentoMensalConverter();
            _converterRetorno = new ParcelamentoConverterRetorno();
            _mensalConverterRetorno = new ParcelamentoMensalConverterRetorno();
        }

        public RetornoParcelamentoDbo ObterPorId(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var parcelamento = _parcelamentoRepository.ObterPorIdSeguro(id, usuarioId);

                if (parcelamento == null)
                    throw new NotFoundException("Parcelamento não encontrado");

                var parcelamentoDbo = _converter.Parse(parcelamento);

                return _converterRetorno.Parse(parcelamentoDbo);
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
                throw new Exception("Erro ao buscar parcelamento por ID.", ex);
            }
        }

        public List<RetornoParcelamentoDbo> ObterTodos(long usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var parcelamentos = _parcelamentoRepository.ObterTodosPorUsuario(usuarioId);

                if (parcelamentos == null || parcelamentos.Count == 0)
                    throw new NotFoundException("Nenhum parcelamento encontrado.");

                var parsedParcelamento = _converter.ParseList(parcelamentos);

                if (parsedParcelamento == null || parsedParcelamento.Count == 0)
                    throw new NotFoundException("Nenhum parcelamento encontrado.");

                return _converterRetorno.ParseList(parsedParcelamento);
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
                throw new Exception("Erro ao buscar todos os parcelamentos.", ex);
            }
        }

        public RetornoParcelamentoDbo Criar(CriarParcelamentoDbo parcelamento, long usuarioId)
        {
            try
            {
                ParcelamentoDbo parcelamentoDbo = new ParcelamentoDbo
                {
                    UsuarioId = usuarioId,
                    Descricao = parcelamento.Descricao,
                    ValorTotal = parcelamento.ValorTotal,
                    NumeroParcelas = parcelamento.NumeroParcelas,
                    DataPrimeiraParcela = parcelamento.DataPrimeiraParcela,
                    IntervaloParcelas = parcelamento.IntervaloParcelas,
                    CartaoId = parcelamento.CartaoId,
                    PessoaContaId = parcelamento.PessoaContaId,
                    Situacao = parcelamento.Situacao
                };

                var parcelamentoEntity = _converter.Parse(parcelamentoDbo);

                if (parcelamentoEntity == null)
                    throw new ArgumentException("Erro ao converter Parcelamento.");

                var criado = _parcelamentoRepository.CriarComCascata(parcelamentoEntity);

                var parcelamentoCriadoDbo = _converter.Parse(criado);

                return _converterRetorno.Parse(parcelamentoCriadoDbo);
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
                throw new Exception("Erro ao criar Parcelamento.", ex);
            }
        }

        public RetornoParcelamentoDbo Atualizar(AtualizarParcelamentoDbo parcelamento, long usuarioId)
        {
            try
            {
                if (parcelamento.Id <= 0)
                    throw new ArgumentException("ID inválido");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var parcelamentoExistente = _parcelamentoRepository.ObterPorIdSeguro(parcelamento.Id, usuarioId);

                if (parcelamentoExistente == null)
                    throw new NotFoundException("Parcelamento não encontrado");

                parcelamentoExistente.Descricao = parcelamento.Descricao;
                parcelamentoExistente.ValorTotal = parcelamento.ValorTotal;
                parcelamentoExistente.NumeroParcelas = parcelamento.NumeroParcelas;
                parcelamentoExistente.DataPrimeiraParcela = parcelamento.DataPrimeiraParcela;
                parcelamentoExistente.IntervaloParcelas = parcelamento.IntervaloParcelas;
                parcelamentoExistente.CartaoId = parcelamento.CartaoId;
                parcelamentoExistente.PessoaContaId = parcelamento.PessoaContaId;
                parcelamentoExistente.Situacao = parcelamento.Situacao;

                var parcelamentoAtualizado = _parcelamentoRepository.AtualizarParcelamentoComCascata(parcelamentoExistente, usuarioId);
                var parcelamentoDboAtualizado = _converter.Parse(parcelamentoAtualizado);

                return _converterRetorno.Parse(parcelamentoDboAtualizado);
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
                throw new Exception("Erro ao atualizar Parcelamento.", ex);
            }
        }

        public List<RetornoParcelamentoMensalDbo> ObterParcelamentosMensaisPorParcelamentoId(long parcelamentoId, long usuarioId)
        {
            try
            {
                if (parcelamentoId <= 0)
                    throw new ArgumentException("O ID do parcelamento deve ser maior que zero.");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var mensais = _parcelamentoRepository.ObterParcelamentosMensaisPorParcelamentoId(parcelamentoId, usuarioId);

                if (mensais == null || mensais.Count == 0)
                    throw new NotFoundException("Nenhum parcelamento mensal encontrado para o parcelamento especificado.");

                var parsedMensais = _mensalConverter.ParseList(mensais);

                return _mensalConverterRetorno.ParseList(parsedMensais);
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
                throw new Exception("Erro ao buscar parcelas mensais do parcelamento.", ex);
            }
        }

        public List<RetornoParcelamentoDbo> ObterPorPessoaContaId(long pessoaContaId, long usuarioId)
        {
            try
            {
                if (pessoaContaId <= 0)
                    throw new ArgumentException("O ID da pessoa deve ser maior que zero.");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var parcelamentos = _parcelamentoRepository.ObterPorPessoaContaId(pessoaContaId, usuarioId);

                if (parcelamentos == null || parcelamentos.Count == 0)
                {
                    throw new NotFoundException("Nenhum parcelamento encontrado para a pessoa especificada.");
                }

                var parsedParcelamentos = _converter.ParseList(parcelamentos);

                return _converterRetorno.ParseList(parsedParcelamentos);
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
                throw new Exception("Erro ao buscar parcelamentos por pessoa.", ex);
            }
        }

        public List<RetornoParcelamentoDbo> ObterPorCartaoId(long cartaoId, long usuarioId)
        {
            try
            {
                if (cartaoId <= 0)
                    throw new ArgumentException("O ID do cartão deve ser maior que zero.");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var parcelamentos = _parcelamentoRepository.ObterPorCartaoId(cartaoId, usuarioId);

                if (parcelamentos == null || parcelamentos.Count == 0)
                    throw new NotFoundException("Nenhum parcelamento encontrado para o cartão especificado.");


                var parsedParcelamentos = _converter.ParseList(parcelamentos);
                
                return _converterRetorno.ParseList(parsedParcelamentos);
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
                throw new Exception("Erro ao buscar parcelamentos por cartão.", ex);
            }
        }

        public List<RetornoParcelamentoDbo> ObterPorPessoaContaIdECartaoId(long pessoaContaId, long cartaoId, long usuarioId)
        {
            try
            {
                if (pessoaContaId <= 0)
                    throw new ArgumentException("O ID da pessoa deve ser maior que zero.");
                if (cartaoId <= 0)
                    throw new ArgumentException("O ID do cartão deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var parcelamentos = _parcelamentoRepository.ObterPorPessoaContaIdECartaoId(pessoaContaId, cartaoId, usuarioId);

                if (parcelamentos == null || parcelamentos.Count == 0)
                    throw new NotFoundException("Nenhum parcelamento encontrado para a pessoa e cartão especificados.");

                var parsedParcelamentos = _converter.ParseList(parcelamentos);

                return _converterRetorno.ParseList(parsedParcelamentos);
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
                throw new Exception("Erro ao buscar parcelamentos por pessoa e cartão.", ex);
            }
        }

        public List<RetornoParcelamentoDbo> ObterPorSituacao(string situacao, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(situacao))
                    throw new ArgumentException("Situação não pode ser vazia.");

                var parcelamentos = _parcelamentoRepository.ObterPorSituacao(situacao, usuarioId);

                if (parcelamentos == null || parcelamentos.Count == 0)
                    throw new NotFoundException("Nenhum parcelamento encontrado para a situação especificada.");

                var parsedParcelamentos = _converter.ParseList(parcelamentos);

                return _converterRetorno.ParseList(parsedParcelamentos);
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
                throw new Exception("Erro ao buscar parcelamentos por situação.", ex);
            }
        }

        public void DeletarParcelamento(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var parcelamentoExistente = _parcelamentoRepository.ObterPorIdSeguro(id, usuarioId);

                if (parcelamentoExistente == null)
                    throw new NotFoundException("Parcelamento não encontrado");

                _parcelamentoRepository.DeletarEmCascata(parcelamentoExistente.Id, usuarioId);
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