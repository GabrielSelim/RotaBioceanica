using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Domain.Entity;

namespace Projeto_Gabriel.Application.Converter.Implementacao
{
    public class PessoaConverter : IParser<PessoaDbo, Pessoa>, IParser<Pessoa, PessoaDbo>
    {
        public Pessoa Parse(PessoaDbo origem)
        {
            if (origem == null) return null;

            return new Pessoa
            {
                Id = origem.Id,
                PrimeiroNome = origem.PrimeiroNome,
                Sobrenome = origem.Sobrenome,
                Endereco = origem.Endereco,
                Sexo = origem.Sexo,
                Ativo = origem.Ativo                
            };
        }

        public PessoaDbo Parse(Pessoa origem)
        {
            if (origem == null) return null;

            return new PessoaDbo
            {
                Id = origem.Id,
                PrimeiroNome = origem.PrimeiroNome,
                Sobrenome = origem.Sobrenome,
                Endereco = origem.Endereco,
                Sexo = origem.Sexo,
                Ativo = origem.Ativo
            };
        }

        public List<Pessoa> ParseList(List<PessoaDbo> origem)
        {
            if (origem == null) return null;

            return origem.Select(item => Parse(item)).ToList();
        }

        public List<PessoaDbo> ParseList(List<Pessoa> origem)
        {
            if (origem == null) return null;

            return origem.Select(item => Parse(item)).ToList();
        }
    }
}
