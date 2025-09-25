using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.PessoaContaDbo;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas
{
    public class PessoaContaConverter : IParser<PessoaContaDbo, PessoaConta>, IParser<PessoaConta, PessoaContaDbo>
    {
        public PessoaConta Parse(PessoaContaDbo origem)
        {
            if (origem == null) return null;
            return new PessoaConta
            {
                Id = origem.Id,
                NomePessoa = origem.NomePessoa,
                UsuarioId = origem.UsuarioId
            };
        }

        public PessoaContaDbo Parse(PessoaConta origem)
        {
            if (origem == null) return null;
            return new PessoaContaDbo
            {
                Id = origem.Id,
                NomePessoa = origem.NomePessoa,
                UsuarioId = origem.UsuarioId
            };
        }

        public List<PessoaConta> ParseList(List<PessoaContaDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }

        public List<PessoaContaDbo> ParseList(List<PessoaConta> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}