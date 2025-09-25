using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Projeto_Gabriel.Infrastructure.Repositorys.Financas
{
    public class ParcelamentoRepository : GenericRepository<Parcelamento>, IParcelamentoRepository
    {
        private static readonly HashSet<string> SituacoesValidas = new HashSet<string> { "Ativo", "Inativo", "Paga", "Não Paga" };

        public ParcelamentoRepository(MySQLContext context) : base(context)
        {
        }

        public Parcelamento ObterPorIdSeguro(long id, long usuarioId)
        {
            return _context.Parcelamentos.FirstOrDefault(p => p.Id == id && p.UsuarioId == usuarioId);
        }

        public List<Parcelamento> ObterTodosPorUsuario(long usuarioId)
        {
            return _context.Parcelamentos.Where(p => p.UsuarioId == usuarioId).ToList();
        }

        public Parcelamento CriarComCascata(Parcelamento parcelamento)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Parcelamentos.Add(parcelamento);
                _context.SaveChanges();

                var categoria = _context.Categorias.FirstOrDefault(c => c.UsuarioId == parcelamento.UsuarioId && c.NomeCategoria == "Parcelamento Mensal");
                if (categoria == null)
                {
                    categoria = new Categoria
                    {
                        UsuarioId = parcelamento.UsuarioId,
                        NomeCategoria = "Parcelamento Mensal",
                        TipoCategoria = "Despesa"
                    };
                    _context.Categorias.Add(categoria);
                    _context.SaveChanges();
                }

                var valorParcela = Math.Round(parcelamento.ValorTotal / parcelamento.NumeroParcelas, 2);
                var listaParcelas = new List<ParcelamentoMensal>();

                for (int i = 0; i < parcelamento.NumeroParcelas; i++)
                {
                    DateTime dataVencimento;
                    if (parcelamento.IntervaloParcelas == 30)
                    {
                        dataVencimento = parcelamento.DataPrimeiraParcela.AddMonths(i);
                    }
                    else
                    {
                        dataVencimento = parcelamento.DataPrimeiraParcela.AddDays(i * parcelamento.IntervaloParcelas);
                    }

                    var mensal = new ParcelamentoMensal
                    {
                        ParcelamentoId = parcelamento.Id,
                        NumeroParcela = i + 1,
                        DataVencimento = dataVencimento,
                        ValorParcela = valorParcela,
                        Situacao = dataVencimento < DateTime.Today ? "Paga" : "Não Paga",
                        CartaoId = parcelamento.CartaoId,
                        PessoaContaId = parcelamento.PessoaContaId
                    };
                    _context.ParcelamentoMensais.Add(mensal);
                    _context.SaveChanges();

                    var lancamento = new Lancamento
                    {
                        UsuarioId = parcelamento.UsuarioId,
                        DataLancamento = mensal.DataVencimento,
                        Descricao = $"{parcelamento.Descricao} - Parcela {mensal.NumeroParcela}/{parcelamento.NumeroParcelas}",
                        Valor = mensal.ValorParcela,
                        TipoLancamento = "Despesa",
                        CategoriaId = categoria.Id,
                        ParcelamentoMensalId = mensal.Id,
                        Situacao = mensal.Situacao
                    };
                    _context.Lancamentos.Add(lancamento);
                    listaParcelas.Add(mensal);
                }

                _context.SaveChanges();
                transaction.Commit();
                parcelamento.ParcelamentosMensais = listaParcelas;
                return parcelamento;
            }
            catch (ValidationException ex)
            {
                transaction.Rollback();
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao criar o parcelamento: " + ex.Message, ex);
            }
        }

        public Parcelamento AtualizarParcelamentoComCascata(Parcelamento parcelamento, long usuarioId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var existente = _context.Parcelamentos
                    .Include(p => p.ParcelamentosMensais)
                    .FirstOrDefault(p => p.Id == parcelamento.Id && p.UsuarioId == usuarioId);

                if (existente == null)
                    throw new ArgumentException("Parcelamento não encontrado para este usuário.");

                existente.Descricao = parcelamento.Descricao;
                existente.ValorTotal = parcelamento.ValorTotal;
                existente.NumeroParcelas = parcelamento.NumeroParcelas;
                existente.DataPrimeiraParcela = parcelamento.DataPrimeiraParcela;
                existente.IntervaloParcelas = parcelamento.IntervaloParcelas;
                existente.PessoaContaId = parcelamento.PessoaContaId;
                existente.CartaoId = parcelamento.CartaoId;

                var mensais = _context.ParcelamentoMensais
                    .Where(pm => pm.ParcelamentoId == existente.Id)
                    .OrderBy(pm => pm.NumeroParcela)
                    .ToList();

                var categoriaLancamento = _context.Categorias
                    .FirstOrDefault(c => c.UsuarioId == existente.UsuarioId && c.NomeCategoria == "Parcelamento Mensal");
                if (categoriaLancamento == null)
                {
                    categoriaLancamento = new Categoria
                    {
                        UsuarioId = existente.UsuarioId,
                        NomeCategoria = "Parcelamento Mensal",
                        TipoCategoria = "Despesa"
                    };
                    _context.Categorias.Add(categoriaLancamento);
                    _context.SaveChanges();
                }

                var novoValorParcela = Math.Round(existente.ValorTotal / existente.NumeroParcelas, 2);

                if (mensais.Count > existente.NumeroParcelas)
                {
                    var mensaisRemover = mensais.Where(pm => pm.NumeroParcela > existente.NumeroParcelas).ToList();
                    foreach (var mensal in mensaisRemover)
                    {
                        var lancamentosRemover = _context.Lancamentos.Where(l => l.ParcelamentoMensalId == mensal.Id).ToList();
                        _context.Lancamentos.RemoveRange(lancamentosRemover);
                    }
                    _context.ParcelamentoMensais.RemoveRange(mensaisRemover);
                    _context.SaveChanges();
                    mensais = mensais.Where(pm => pm.NumeroParcela <= existente.NumeroParcelas).ToList();
                }

                for (int i = 0; i < mensais.Count; i++)
                {
                    var mensal = mensais[i];
                    DateTime dataVencimento;
                    if (existente.IntervaloParcelas == 30)
                        dataVencimento = existente.DataPrimeiraParcela.AddMonths(i);
                    else
                        dataVencimento = existente.DataPrimeiraParcela.AddDays(i * existente.IntervaloParcelas);

                    mensal.NumeroParcela = i + 1;
                    mensal.DataVencimento = dataVencimento;
                    mensal.ValorParcela = novoValorParcela;

                    var lancamento = _context.Lancamentos.FirstOrDefault(l => l.ParcelamentoMensalId == mensal.Id);
                    if (lancamento != null)
                    {
                        lancamento.Descricao = $"{existente.Descricao} - Parcela {mensal.NumeroParcela}/{existente.NumeroParcelas}";
                        lancamento.TipoLancamento = "Despesa";
                        lancamento.CategoriaId = categoriaLancamento.Id;
                        lancamento.Valor = novoValorParcela;
                        lancamento.DataLancamento = mensal.DataVencimento;
                    }
                }

                for (int i = mensais.Count; i < existente.NumeroParcelas; i++)
                {
                    DateTime dataVencimento;
                    if (existente.IntervaloParcelas == 30)
                        dataVencimento = existente.DataPrimeiraParcela.AddMonths(i);
                    else
                        dataVencimento = existente.DataPrimeiraParcela.AddDays(i * existente.IntervaloParcelas);

                    var mensal = new ParcelamentoMensal
                    {
                        ParcelamentoId = existente.Id,
                        NumeroParcela = i + 1,
                        DataVencimento = dataVencimento,
                        ValorParcela = novoValorParcela,
                        Situacao = dataVencimento < DateTime.Today ? "Paga" : "Não Paga",
                        CartaoId = existente.CartaoId,
                        PessoaContaId = existente.PessoaContaId
                    };
                    _context.ParcelamentoMensais.Add(mensal);
                    _context.SaveChanges();

                    var lancamento = new Lancamento
                    {
                        UsuarioId = existente.UsuarioId,
                        DataLancamento = mensal.DataVencimento,
                        Descricao = $"{existente.Descricao} - Parcela {mensal.NumeroParcela}/{existente.NumeroParcelas}",
                        Valor = mensal.ValorParcela,
                        TipoLancamento = "Despesa",
                        CategoriaId = categoriaLancamento.Id,
                        ParcelamentoMensalId = mensal.Id,
                        Situacao = mensal.Situacao
                    };
                    _context.Lancamentos.Add(lancamento);
                }

                _context.SaveChanges();
                transaction.Commit();
                return existente;
            }
            catch (ValidationException ex)
            {
                transaction.Rollback();
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao atualizar o parcelamento: " + ex.Message, ex);
            }
        }

        public void DeletarEmCascata(long parcelamentoId, long usuarioId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var parcelamento = _context.Parcelamentos
                    .FirstOrDefault(p => p.Id == parcelamentoId && p.UsuarioId == usuarioId);

                if (parcelamento == null)
                    throw new ArgumentException("Parcelamento não encontrado para este usuário.");

                var mensais = _context.ParcelamentoMensais
                    .Where(pm => pm.ParcelamentoId == parcelamento.Id)
                    .ToList();

                var mensalIds = mensais.Select(pm => pm.Id).ToList();
                var lancamentos = _context.Lancamentos
                    .Where(l => mensalIds.Contains(l.ParcelamentoMensalId ?? 0))
                    .ToList();

                _context.Lancamentos.RemoveRange(lancamentos);

                _context.ParcelamentoMensais.RemoveRange(mensais);

                _context.Parcelamentos.Remove(parcelamento);

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao deletar parcelamento em cascata: " + ex.Message, ex);
            }
        }

        public List<ParcelamentoMensal> ObterParcelamentosMensaisPorParcelamentoId(long parcelamentoId, long usuarioId)
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

        public List<Parcelamento> ObterPorPessoaContaId(long pessoaContaId, long usuarioId)
        {
            try
            {
                if (pessoaContaId <= 0)
                    throw new ArgumentException("O ID da pessoa deve ser maior que zero.");

                if (!_context.PessoaContas.Any(p => p.Id == pessoaContaId))
                    throw new ArgumentException($"PessoaConta com ID {pessoaContaId} não existe.");

                return _context.Parcelamentos
                    .Where(p => p.PessoaContaId == pessoaContaId && p.UsuarioId == usuarioId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar parcelamentos por pessoa: " + ex.Message, ex);
            }
        }

        public List<Parcelamento> ObterPorCartaoId(long cartaoId, long usuarioId)
        {
            try
            {
                if (cartaoId <= 0)
                    throw new ArgumentException("O ID do cartão deve ser maior que zero.");

                if (!_context.Cartoes.Any(c => c.Id == cartaoId))
                    throw new ArgumentException($"Cartão com ID {cartaoId} não existe.");

                return _context.Parcelamentos
                    .Where(p => p.CartaoId == cartaoId && p.UsuarioId == usuarioId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar parcelamentos por cartão: " + ex.Message, ex);
            }
        }

        public List<Parcelamento> ObterPorPessoaContaIdECartaoId(long pessoaContaId, long cartaoId, long usuarioId)
        {
            try
            {
                if (pessoaContaId <= 0)
                    throw new ArgumentException("O ID da pessoa deve ser maior que zero.");

                if (cartaoId <= 0)
                    throw new ArgumentException("O ID do cartão deve ser maior que zero.");

                if (!_context.PessoaContas.Any(p => p.Id == pessoaContaId))
                    throw new ArgumentException($"PessoaConta com ID {pessoaContaId} não existe.");

                if (!_context.Cartoes.Any(c => c.Id == cartaoId))
                    throw new ArgumentException($"Cartão com ID {cartaoId} não existe.");

                return _context.Parcelamentos
                    .Where(p => p.PessoaContaId == pessoaContaId && p.CartaoId == cartaoId && p.UsuarioId == usuarioId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar parcelamentos por pessoa e cartão: " + ex.Message, ex);
            }
        }

        public List<Parcelamento> ObterPorSituacao(string situacao, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(situacao) || !SituacoesValidas.Contains(situacao))
                    throw new ArgumentException($"Situação '{situacao}' não é válida. Situações válidas: {string.Join(", ", SituacoesValidas)}");

                return _context.Parcelamentos
                    .Where(p => p.Situacao == situacao && p.UsuarioId == usuarioId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar parcelamentos por situação: " + ex.Message, ex);
            }
        }
    }
}