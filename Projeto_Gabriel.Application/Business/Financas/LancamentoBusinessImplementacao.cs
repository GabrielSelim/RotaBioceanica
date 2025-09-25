using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas;
using Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;

namespace Projeto_Gabriel.Application.Business.Financas
{
    public class LancamentoBusinessImplementacao : ILancamentoBusiness
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly LancamentoConverter _converter;
        private readonly LancamentoConverterRetorno _converterRetorno;

        public LancamentoBusinessImplementacao(ILancamentoRepository lancamentoRepository)
        {
            _lancamentoRepository = lancamentoRepository;
            _converter = new LancamentoConverter();
            _converterRetorno = new LancamentoConverterRetorno();
        }

        public RetornoLancamentoDbo ObterPorId(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var lancamento = _lancamentoRepository.ObterPorIdSeguro(id, usuarioId);

                if (lancamento == null)
                    throw new NotFoundException("Lançamento não encontrado");

                var parsedLancamento = _converter.Parse(lancamento);

                return _converterRetorno.Parse(parsedLancamento);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamento por ID.", ex);
            }
        }

        public List<RetornoLancamentoDbo> ObterTodos(long usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var lancamentos = _lancamentoRepository.ObterTodosSeguro(usuarioId);

                if (lancamentos == null || lancamentos.Count == 0)
                    throw new NotFoundException("Nenhum lançamento encontrado.");

                var parsed = _converter.ParseList(lancamentos);

                if (parsed == null || parsed.Count == 0)
                    throw new NotFoundException("Nenhum lançamento encontrado.");

                return _converterRetorno.ParseList(parsed);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar todos os lançamentos.", ex);
            }
        }

        public RetornoLancamentoDbo Criar(CriarLancamentoDbo lancamento, long usuarioId)
        {
            try
            {
                LancamentoDbo lancamentoDbo = new LancamentoDbo()
                {
                    DataLancamento = lancamento.DataLancamento,
                    UsuarioId = usuarioId,
                    Descricao = lancamento.Descricao,
                    Valor = lancamento.Valor,
                    TipoLancamento = lancamento.TipoLancamento,
                    CategoriaId = lancamento.CategoriaId,
                    Situacao = lancamento.Situacao
                };

                var lancamentoEntity = _converter.Parse(lancamentoDbo);

                if (lancamentoEntity == null)
                    throw new ArgumentException("Erro ao converter Lançamento.");
                
                var lancamentoCriado = _lancamentoRepository.Criar(lancamentoEntity);

                if (lancamentoCriado == null)
                    throw new NotFoundException("Erro ao criar Lançamento.");

                var lancamentoDboCriado = _converter.Parse(lancamentoCriado);

                return _converterRetorno.Parse(lancamentoDboCriado);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar Lançamento.", ex);
            }
        }

        public List<RetornoLancamentoDbo> ObterPorCategoriaId(long categoriaId, long usuarioId)
        {
            try
            {
                if (categoriaId <= 0)
                    throw new ArgumentException("O ID da categoria deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var lancamentos = _lancamentoRepository.ObterPorCategoriaId((int)categoriaId, usuarioId);

                if (lancamentos == null || lancamentos.Count == 0)
                    throw new NotFoundException("Nenhum lançamento encontrado para a categoria especificada.");

                var parsedLancamentos = _converter.ParseList(lancamentos);

                return _converterRetorno.ParseList(parsedLancamentos);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamentos por categoria.", ex);
            }
        }

        public List<RetornoLancamentoDbo> ObterPorParcelamentoMensalId(long parcelamentoMensalId, long usuarioId)
        {
            try
            {
                if (parcelamentoMensalId <= 0)
                    throw new ArgumentException("O ID do Parcelamento Mensal deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var lancamentos = _lancamentoRepository.ObterPorParcelamentoMensalId((int)parcelamentoMensalId, usuarioId);

                if (lancamentos == null || lancamentos.Count == 0)
                    throw new NotFoundException("Nenhum lançamento encontrado para a parcela mensal especificada.");

                var parsedLancamentos = _converter.ParseList(lancamentos);

                return _converterRetorno.ParseList(parsedLancamentos);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamentos por parcela mensal.", ex);
            }
        }

        public List<RetornoLancamentoDbo> ObterPorPeriodo(long usuarioId, DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("O ID do usuário deve ser maior que zero.");

                if (dataFim < dataInicio)
                    throw new ArgumentException("A data final não pode ser menor que a data inicial.");

                var lancamentos = _lancamentoRepository.ObterPorPeriodo(usuarioId, dataInicio, dataFim);

                if (lancamentos == null || lancamentos.Count == 0)
                    throw new NotFoundException("Nenhum lançamento encontrado para o período especificado.");

                var parsedLancamentos = _converter.ParseList(lancamentos);

                return _converterRetorno.ParseList(parsedLancamentos);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamentos por período.", ex);
            }
        }

        public List<RetornoLancamentoDbo> ObterPorSituacao(string situacao, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(situacao))
                    throw new ArgumentException("Situação não pode ser vazia.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var lancamentos = _lancamentoRepository.ObterPorSituacao(situacao, usuarioId);

                if (lancamentos == null || lancamentos.Count == 0)
                    throw new NotFoundException("Nenhum lançamento encontrado para a situação especificada.");

                var parsedLancamentos = _converter.ParseList(lancamentos);

                return _converterRetorno.ParseList(parsedLancamentos);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamentos por situação.", ex);
            }
        }

        public void MarcarComoPago(MarcarComoPagoRequest request, long usuarioId)
        {
            try
            {
                if (request.Id <= 0)
                    throw new ArgumentException("O ID do lançamento deve ser maior que zero.");
                if (request.ValorPago <= 0)
                    throw new ArgumentException("O valor pago deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                _lancamentoRepository.MarcarComoPago(request.Id, request.ValorPago, request.DataPagamento, usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao marcar lançamento como pago: " + ex.Message, ex);
            }
        }

        public RetornoLancamentoDbo Atualizar(AtualizarLancamentoDbo atualizar, long usuarioId)
        {
            try
            {
                if (atualizar.Id <= 0)
                    throw new ArgumentException("ID inválido");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var lancamentoExistente = _lancamentoRepository.ObterPorIdSeguro(atualizar.Id, usuarioId);

                if (lancamentoExistente == null)
                    throw new NotFoundException("Lançamento não encontrado");

                if (lancamentoExistente.ParcelamentoMensalId.HasValue)
                    throw new ArgumentException("Não é possível deletar um lançamento vinculado a um parcelamento mensal.");


                lancamentoExistente.DataLancamento = atualizar.DataLancamento;
                lancamentoExistente.Descricao = atualizar.Descricao;
                lancamentoExistente.Valor = atualizar.Valor;
                lancamentoExistente.TipoLancamento = atualizar.TipoLancamento;
                lancamentoExistente.CategoriaId = atualizar.CategoriaId;
                lancamentoExistente.Situacao = atualizar.Situacao;

                var lancamentoAtualizado = _lancamentoRepository.Atualizar(lancamentoExistente);

                if (lancamentoAtualizado == null)
                    throw new NotFoundException("Erro ao atualizar Lançamento.");

                var parsedLancamento = _converter.Parse(lancamentoAtualizado);

                return _converterRetorno.Parse(parsedLancamento);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Lançamento.", ex);
            }
        }

        public void Deletar(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("O ID do lançamento deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var lancamentoExistente = _lancamentoRepository.ObterPorIdSeguro(id, usuarioId);
                if (lancamentoExistente == null)
                    throw new NotFoundException("Lançamento não encontrado");

                if (lancamentoExistente.Situacao == "Pago")
                    throw new ArgumentException("Não é possível deletar um lançamento que já foi pago.");

                if (lancamentoExistente.ParcelamentoMensalId.HasValue)
                    throw new ArgumentException("Não é possível deletar um lançamento vinculado a um parcelamento mensal.");

                _lancamentoRepository.Deletar(lancamentoExistente.Id);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Erro de operação inválida: {ex.Message}", ex);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}