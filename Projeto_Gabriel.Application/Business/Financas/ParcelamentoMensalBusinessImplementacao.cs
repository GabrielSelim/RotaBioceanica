using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas;
using Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoMensalDbo;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;

namespace Projeto_Gabriel.Application.Business.Financas
{
    public class ParcelamentoMensalBusinessImplementacao : IParcelamentoMensalBusiness
    {
        private readonly IParcelamentoMensalRepository _mensalRepository;
        private readonly ParcelamentoMensalConverter _converter;
        private readonly ParcelamentoMensalConverterRetorno _converterRetorno;

        public ParcelamentoMensalBusinessImplementacao(IParcelamentoMensalRepository mensalRepository)
        {
            _mensalRepository = mensalRepository;
            _converter = new ParcelamentoMensalConverter();
            _converterRetorno = new ParcelamentoMensalConverterRetorno();
        }

        public RetornoParcelamentoMensalDbo ObterPorId(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var mensal = _mensalRepository.ObterPorIdSeguro(id, usuarioId);

                if (mensal == null)
                    throw new NotFoundException("Parcelamento Mensal não encontrado");

                var parsed = _converter.Parse(mensal);

                return _converterRetorno.Parse(parsed);
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
                throw new Exception("Erro ao buscar Parcelamento Mensal por ID.", ex);
            }
        }

        public List<RetornoParcelamentoMensalDbo> ObterTodos(long usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var mensais = _mensalRepository.ObterTodosSeguro(usuarioId);

                if (mensais == null || mensais.Count == 0)
                    throw new NotFoundException("Nenhum Parcelamento Mensal encontrado.");

                var parsed = _converter.ParseList(mensais);

                if (parsed == null || parsed.Count == 0)
                    throw new NotFoundException("Nenhum Parcelamento Mensal encontrado.");

                return _converterRetorno.ParseList(parsed);
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
                throw new Exception("Erro ao buscar todos os ParcelamentoMensal.", ex);
            }
        }

        public List<RetornoParcelamentoMensalDbo> ObterPorParcelamentoId(long parcelamentoId, long usuarioId)
        {
            try
            {
                if (parcelamentoId <= 0)
                    throw new ArgumentException("O ID do parcelamento deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var mensais = _mensalRepository.ObterPorParcelamentoId(parcelamentoId, usuarioId);

                if (mensais == null || mensais.Count == 0)
                    throw new NotFoundException("Nenhum Parcelamento Mensal encontrado para o parcelamento especificado.");

                var parsedMensais = _converter.ParseList(mensais);

                return _converterRetorno.ParseList(parsedMensais);
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
                throw new Exception("Erro ao buscar ParcelamentoMensal por parcelamentoId.", ex);
            }
        }

        public List<RetornoParcelamentoMensalDbo> ObterPorSituacao(string situacao, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(situacao))
                    throw new ArgumentException("Situação não pode ser vazia.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var mensais = _mensalRepository.ObterPorSituacao(situacao, usuarioId);

                if (mensais == null || mensais.Count == 0)
                    throw new NotFoundException("Nenhum Parcelamento Mensal encontrado para a situação especificada.");

                var parsedMensais = _converter.ParseList(mensais);

                return _converterRetorno.ParseList(parsedMensais);
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
                throw new Exception("Erro ao buscar ParcelamentoMensal por situação.", ex);
            }
        }

        public List<RetornoParcelamentoMensalDbo> ObterPorPessoaContaId(long pessoaContaId, long usuarioId)
        {
            try
            {
                if (pessoaContaId <= 0)
                    throw new ArgumentException("O ID da pessoa deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var mensais = _mensalRepository.ObterPorPessoaContaId(pessoaContaId, usuarioId);

                if (mensais == null || mensais.Count == 0)
                    throw new NotFoundException("Nenhum Parcelamento Mensal encontrado para a pessoa especificada.");

                var parsedMensais = _converter.ParseList(mensais);

                return _converterRetorno.ParseList(parsedMensais);
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
                throw new Exception("Erro ao buscar ParcelamentoMensal por pessoa.", ex);
            }
        }

        public List<RetornoParcelamentoMensalDbo> ObterPorCartaoId(long cartaoId, long usuarioId)
        {
            try
            {
                if (cartaoId <= 0)
                    throw new ArgumentException("O ID do cartão deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var mensais = _mensalRepository.ObterPorCartaoId((int)cartaoId, usuarioId);

                if (mensais == null || mensais.Count == 0)
                    throw new NotFoundException("Nenhum ParcelamentoMensal encontrado para o cartão especificado.");

                var parsedMensais = _converter.ParseList(mensais);

                return _converterRetorno.ParseList(parsedMensais);
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
                throw new Exception("Erro ao buscar ParcelamentoMensal por cartão.", ex);
            }
        }

        public void MarcarComoPago(MarcarComoPagoRequest request, long usuarioId)
        {
            try
            {
                if (request.Id <= 0)
                    throw new ArgumentException("ID inválido");
                if (request.ValorPago <= 0)
                    throw new ArgumentException("O valor pago deve ser maior que zero.");
                if (request.DataPagamento == default)
                    throw new ArgumentException("Data de pagamento inválida.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var mensal = _mensalRepository.ObterPorIdSeguro(request.Id, usuarioId);

                if (mensal == null)
                    throw new NotFoundException("Parcelamento Mensal não encontrado");

                _mensalRepository.MarcarComoPago(request.Id, request.ValorPago, request.DataPagamento, usuarioId);
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
                throw new Exception("Erro ao marcar parcela como paga: " + ex.Message, ex);
            }
        }

        public void MarcarComoInativo(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var mensal = _mensalRepository.ObterPorIdSeguro(id, usuarioId);

                if (mensal == null)
                    throw new NotFoundException("Parcelamento Mensal não encontrado");

                _mensalRepository.MarcarComoInativo(id, usuarioId);
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
                throw new Exception("Erro ao marcar parcela como inativa: " + ex.Message, ex);
            }
        }
    }
}