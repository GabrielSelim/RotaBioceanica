using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Domain.Entity;

namespace Projeto_Gabriel.Application.Converter.Implementacao
{
    public class LivroConverter : IParser<LivrosDbo, Livros>, IParser<Livros, LivrosDbo>
    {
        public Livros Parse(LivrosDbo origem)
        {
            if (origem == null) return null;

            return new Livros
            {
                Id = origem.Id,
                Autor = origem.Autor,
                DataLancamento = origem.DataLancamento,
                Preco = origem.Preco,
                Titulo = origem.Titulo,
                Ativo = origem.Ativo
            };
        }

        public LivrosDbo Parse(Livros origem)
        {
            if (origem == null) return null;

            return new LivrosDbo
            {
                Id = origem.Id,
                Autor = origem.Autor,
                DataLancamento = origem.DataLancamento,
                Preco = origem.Preco,
                Titulo = origem.Titulo,
                Ativo = origem.Ativo
            };
        }

        public List<Livros> ParseList(List<LivrosDbo> origem)
        {
            if (origem == null) return null;

            return origem.Select(item => Parse(item)).ToList();
        }

        public List<LivrosDbo> ParseList(List<Livros> origem)
        {
            if (origem == null) return null;

            return origem.Select(item => Parse(item)).ToList();
        }
    }
}
