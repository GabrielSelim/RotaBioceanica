using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Infrastructure.Repositorys.Financas
{
    public class ParcelamentoMensalRepository : GenericRepository<ParcelamentoMensal>, IParcelamentoMensalRepository
    {
        private static readonly HashSet<string> SituacoesValidas = new HashSet<string> { "Paga", "Não Paga", "Inativo" };

        public ParcelamentoMensalRepository(MySQLContext context) : base(context)
        {
        }

        public List<ParcelamentoMensal> ObterTodosSeguro(long usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            try
            {
                return _context.ParcelamentoMensais
                    .Where(pm => _context.Parcelamentos.Any(p => p.Id == pm.ParcelamentoId && p.UsuarioId == usuarioId))
                    .OrderBy(pm => pm.DataVencimento)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar todas as parcelas mensais do usuário: " + ex.Message, ex);
            }
        }

        public ParcelamentoMensal? ObterPorIdSeguro(long id, long usuarioId)
        {
            if (id <= 0)
                throw new ArgumentException("ID da parcela mensal inválido.");
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            try
            {
                return _context.ParcelamentoMensais
                    .FirstOrDefault(pm => pm.Id == id &&
                        _context.Parcelamentos.Any(p => p.Id == pm.ParcelamentoId && p.UsuarioId == usuarioId));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar parcela mensal por ID do usuário: " + ex.Message, ex);
            }
        }

        public List<ParcelamentoMensal> ObterPorParcelamentoId(long parcelamentoId, long usuarioId)
        {
            try
            {
                if (parcelamentoId <= 0)
                    throw new ArgumentException("O ID do parcelamento deve ser maior que zero.");

                if (!_context.Parcelamentos.Any(p => p.Id == parcelamentoId && p.UsuarioId == usuarioId))
                    throw new ArgumentException($"Parcelamento com ID {parcelamentoId} não existe para este usuário.");

                return _context.ParcelamentoMensais
                    .Where(pm => pm.ParcelamentoId == parcelamentoId)
                    .OrderBy(pm => pm.NumeroParcela)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar parcelas mensais: " + ex.Message, ex);
            }
        }

        public List<ParcelamentoMensal> ObterPorSituacao(string situacao, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(situacao) || !SituacoesValidas.Contains(situacao))
                    throw new ArgumentException($"Situação '{situacao}' não é válida. Situações válidas: {string.Join(", ", SituacoesValidas)}");

                return _context.ParcelamentoMensais
                    .Where(pm =>
                        pm.Situacao == situacao &&
                        _context.Parcelamentos.Any(p => p.Id == pm.ParcelamentoId && p.UsuarioId == usuarioId)
                    )
                    .OrderBy(pm => pm.DataVencimento)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar parcelas mensais por situação: " + ex.Message, ex);
            }
        }

        public List<ParcelamentoMensal> ObterPorPessoaContaId(long pessoaContaId, long usuarioId)
        {
            try
            {
                if (pessoaContaId <= 0)
                    throw new ArgumentException("O ID da pessoa deve ser maior que zero.");

                if (!_context.PessoaContas.Any(p => p.Id == pessoaContaId))
                    throw new ArgumentException($"PessoaConta com ID {pessoaContaId} não existe.");

                return _context.ParcelamentoMensais
                    .Where(pm =>
                        pm.PessoaContaId == pessoaContaId &&
                        _context.Parcelamentos.Any(p => p.Id == pm.ParcelamentoId && p.UsuarioId == usuarioId)
                    )
                    .OrderBy(pm => pm.DataVencimento)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar parcelas mensais por pessoa: " + ex.Message, ex);
            }
        }

        public List<ParcelamentoMensal> ObterPorCartaoId(long cartaoId, long usuarioId)
        {
            try
            {
                if (cartaoId <= 0)
                    throw new ArgumentException("O ID do cartão deve ser maior que zero.");

                if (!_context.Cartoes.Any(c => c.Id == cartaoId))
                    throw new ArgumentException($"Cartão com ID {cartaoId} não existe.");

                return _context.ParcelamentoMensais
                    .Where(pm =>
                        pm.CartaoId == cartaoId &&
                        _context.Parcelamentos.Any(p => p.Id == pm.ParcelamentoId && p.UsuarioId == usuarioId)
                    )
                    .OrderBy(pm => pm.DataVencimento)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar parcelas mensais por cartão: " + ex.Message, ex);
            }
        }

        public void MarcarComoPago(long id, decimal valorPago, DateTime dataPagamento, long usuarioId)
        {
            try
            {
                var mensal = _context.ParcelamentoMensais.FirstOrDefault(pm =>
                    pm.Id == id &&
                    _context.Parcelamentos.Any(p => p.Id == pm.ParcelamentoId && p.UsuarioId == usuarioId)
                );
                if (mensal == null)
                    throw new ArgumentException($"Parcela mensal com ID {id} não encontrada para este usuário.");

                mensal.ValorPago = valorPago;
                mensal.DataPagamento = dataPagamento;
                mensal.Situacao = "Paga";

                _context.ParcelamentoMensais.Update(mensal);
                _context.SaveChanges();
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
                var mensal = _context.ParcelamentoMensais.FirstOrDefault(pm =>
                    pm.Id == id &&
                    _context.Parcelamentos.Any(p => p.Id == pm.ParcelamentoId && p.UsuarioId == usuarioId)
                );
                if (mensal == null)
                    throw new ArgumentException($"Parcela mensal com ID {id} não encontrada para este usuário.");

                mensal.Situacao = "Inativo";
                _context.ParcelamentoMensais.Update(mensal);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao marcar parcela como inativa: " + ex.Message, ex);
            }
        }
    }
}