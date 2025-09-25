using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Infrastructure.Repositorys.Financas
{
    public class LancamentoRepository : GenericRepository<Lancamento>, ILancamentoRepository
    {
        public LancamentoRepository(MySQLContext context) : base(context)
        {
        }

        public List<Lancamento> ObterTodosSeguro(long usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            try
            {
                return _context.Lancamentos
                    .Where(l => l.UsuarioId == usuarioId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar todos os lançamentos do usuário: " + ex.Message, ex);
            }
        }

        public Lancamento? ObterPorIdSeguro(long id, long usuarioId)
        {
            if (id <= 0)
                throw new ArgumentException("ID do lançamento inválido.");
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            try
            {
                return _context.Lancamentos
                    .FirstOrDefault(l => l.Id == id && l.UsuarioId == usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamento por ID do usuário: " + ex.Message, ex);
            }
        }

        public List<Lancamento> ObterPorUsuarioId(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("O ID do usuário deve ser maior que zero.");

                return _context.Lancamentos
                    .Where(l => l.UsuarioId == usuarioId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamentos por usuário: " + ex.Message, ex);
            }
        }

        public List<Lancamento> ObterPorCategoriaId(int categoriaId, long usuarioId)
        {
            try
            {
                if (categoriaId <= 0)
                    throw new ArgumentException("O ID da categoria deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido.");

                return _context.Lancamentos
                    .Where(l => l.CategoriaId == categoriaId && l.UsuarioId == usuarioId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamentos por categoria: " + ex.Message, ex);
            }
        }

        public List<Lancamento> ObterPorParcelamentoMensalId(int parcelamentoMensalId, long usuarioId)
        {
            try
            {
                if (parcelamentoMensalId <= 0)
                    throw new ArgumentException("O ID do ParcelamentoMensal deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido.");

                return _context.Lancamentos
                    .Where(l => l.ParcelamentoMensalId == parcelamentoMensalId && l.UsuarioId == usuarioId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamentos por parcela mensal: " + ex.Message, ex);
            }
        }

        public List<Lancamento> ObterPorPeriodo(long usuarioId, DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("O ID do usuário deve ser maior que zero.");

                if (dataFim < dataInicio)
                    throw new ArgumentException("A data final não pode ser menor que a data inicial.");

                return _context.Lancamentos
                    .Where(l => l.UsuarioId == usuarioId && l.DataLancamento >= dataInicio && l.DataLancamento <= dataFim)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamentos por período: " + ex.Message, ex);
            }
        }

        public List<Lancamento> ObterPorSituacao(string situacao, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(situacao))
                    throw new ArgumentException("Situação não pode ser vazia.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido.");

                return _context.Lancamentos
                    .Where(l => l.Situacao == situacao && l.UsuarioId == usuarioId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar lançamentos por situação: " + ex.Message, ex);
            }
        }

        public void MarcarComoPago(long lancamentoId, decimal valorPago, DateTime dataPagamento, long usuarioId)
        {
            try
            {
                if (lancamentoId <= 0)
                    throw new ArgumentException("O ID do lançamento deve ser maior que zero.");
                if (valorPago <= 0)
                    throw new ArgumentException("O valor pago deve ser maior que zero.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido.");

                var lancamento = _context.Lancamentos.FirstOrDefault(l => l.Id == lancamentoId && l.UsuarioId == usuarioId);
                if (lancamento == null)
                    throw new ArgumentException($"Lançamento com ID {lancamentoId} não encontrado para este usuário.");

                lancamento.Valor = valorPago;
                lancamento.DataLancamento = dataPagamento;
                lancamento.Situacao = "Paga";
                _context.Lancamentos.Update(lancamento);

                if (lancamento.ParcelamentoMensalId.HasValue)
                {
                    var mensal = _context.ParcelamentoMensais.FirstOrDefault(pm => pm.Id == lancamento.ParcelamentoMensalId.Value);
                    if (mensal != null)
                    {
                        mensal.ValorPago = valorPago;
                        mensal.DataPagamento = dataPagamento;
                        mensal.Situacao = "Paga";
                        _context.ParcelamentoMensais.Update(mensal);
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao marcar lançamento como pago: " + ex.Message, ex);
            }
        }
    }
}

