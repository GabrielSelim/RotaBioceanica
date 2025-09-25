using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.CategoriaDbo;
using Projeto_Gabriel.Domain.Entity.Financas;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas
{
    public class CategoriaConverter : IParser<CategoriaDbo, Categoria>, IParser<Categoria, CategoriaDbo>
    {
        public Categoria Parse(CategoriaDbo origem)
        {
            if (origem == null) return null;
            return new Categoria
            {
                Id = origem.Id,
                UsuarioId = origem.UsuarioId,
                NomeCategoria = origem.NomeCategoria,
                TipoCategoria = origem.TipoCategoria
            };
        }

        public CategoriaDbo Parse(Categoria origem)
        {
            if (origem == null) return null;
            return new CategoriaDbo
            {
                Id = origem.Id,
                UsuarioId = origem.UsuarioId,
                NomeCategoria = origem.NomeCategoria,
                TipoCategoria = origem.TipoCategoria
            };
        }

        public List<Categoria> ParseList(List<CategoriaDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }

        public List<CategoriaDbo> ParseList(List<Categoria> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}