using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.PessoaContaDbo;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas
{
    public class PessoaContaConverterRetorno : IParser<PessoaContaDbo , RetornoPessoaContaDbo>, IParser<RetornoPessoaContaDbo, PessoaContaDbo>
    {
        public RetornoPessoaContaDbo Parse(PessoaContaDbo origem)
        {
            if (origem == null) return null;
            return new RetornoPessoaContaDbo
            {
                Id = origem.Id,
                NomePessoa = origem.NomePessoa,
            };
        }

        public PessoaContaDbo Parse(RetornoPessoaContaDbo origem)
        {
            if (origem == null) return null;
            return new PessoaContaDbo
            {
                Id = origem.Id,
                NomePessoa = origem.NomePessoa,
            };
        }

        public List<RetornoPessoaContaDbo> ParseList(List<PessoaContaDbo> origem)
        {
            if (origem == null || !origem.Any()) return new List<RetornoPessoaContaDbo>();
            return origem.Select(Parse).ToList();
        }

        public List<PessoaContaDbo> ParseList(List<RetornoPessoaContaDbo> origem)
        {
            if (origem == null || !origem.Any()) return new List<PessoaContaDbo>();
            return origem.Select(Parse).ToList();
        }
    }
}