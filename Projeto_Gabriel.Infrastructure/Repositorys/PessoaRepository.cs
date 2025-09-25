using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.RepositoryInterface;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Infrastructure.Repositorys
{
    public class PessoaRepository : GenericRepository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(MySQLContext context) : base(context)
        {
        }

        public List<Pessoa> ObterPessoasPorEndereco(string endereco)
        {
            if (string.IsNullOrEmpty(endereco)) return null;
            return _context.Pessoas.Where(p => p.Endereco.Contains(endereco)).ToList();
        }

        public List<Pessoa> ObterPessoasPorNome(string nome)
        {
            if (string.IsNullOrEmpty(nome)) return null;
            return _context.Pessoas.Where(p => p.PrimeiroNome.Contains(nome)).ToList();
        }

        public Pessoa Desativar(long id)
        {
            return AlterarStatus(id, false);
        }

        public Pessoa Ativar(long id)
        {
            return AlterarStatus(id, true);
        }

        public Pessoa AlterarStatus(long id, bool status)
        {
            var pessoa = _context.Pessoas.FirstOrDefault(p => p.Id.Equals(id));

            if (pessoa != null)
            {
                try
                {
                    pessoa.Ativo = status;
                    _context.Pessoas.Update(pessoa);
                    _context.SaveChanges();
                    return pessoa;
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