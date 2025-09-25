using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoDbo;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas
{
    public class ParcelamentoConverter : IParser<ParcelamentoDbo, Parcelamento>, IParser<Parcelamento, ParcelamentoDbo>
    {
        public Parcelamento Parse(ParcelamentoDbo origem)
        {
            if (origem == null) return null;
            return new Parcelamento
            {
                Id = origem.Id,
                UsuarioId = origem.UsuarioId,
                Descricao = origem.Descricao,
                ValorTotal = origem.ValorTotal,
                NumeroParcelas = origem.NumeroParcelas,
                DataPrimeiraParcela = origem.DataPrimeiraParcela,
                IntervaloParcelas = origem.IntervaloParcelas,
                CartaoId = origem.CartaoId,
                PessoaContaId = origem.PessoaContaId,
                Situacao = origem.Situacao
            };
        }

        public ParcelamentoDbo Parse(Parcelamento origem)
        {
            if (origem == null) return null;
            return new ParcelamentoDbo
            {
                Id = origem.Id,
                UsuarioId = origem.UsuarioId,
                Descricao = origem.Descricao,
                ValorTotal = origem.ValorTotal,
                NumeroParcelas = origem.NumeroParcelas,
                DataPrimeiraParcela = origem.DataPrimeiraParcela,
                IntervaloParcelas = origem.IntervaloParcelas,
                CartaoId = origem.CartaoId,
                PessoaContaId = origem.PessoaContaId,
                Situacao = origem.Situacao
            };
        }

        public List<Parcelamento> ParseList(List<ParcelamentoDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }

        public List<ParcelamentoDbo> ParseList(List<Parcelamento> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}