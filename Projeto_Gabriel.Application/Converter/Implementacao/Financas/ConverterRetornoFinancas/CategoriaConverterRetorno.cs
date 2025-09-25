using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Financas.CategoriaDbo;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas
{
    public class CategoriaConverterRetorno : IParser<CategoriaDbo, RetornoCategoriaDbo>, IParser<RetornoCategoriaDbo, CategoriaDbo>
    {
        public RetornoCategoriaDbo Parse(CategoriaDbo origem)
        {
            if (origem == null) return null;
            return new RetornoCategoriaDbo
            {
                Id = origem.Id,
                NomeCategoria = origem.NomeCategoria,
                TipoCategoria = origem.TipoCategoria
            };
        }
        public CategoriaDbo Parse(RetornoCategoriaDbo origem)
        {
            if (origem == null) return null;
            return new CategoriaDbo
            {
                Id = origem.Id,
                NomeCategoria = origem.NomeCategoria,
                TipoCategoria = origem.TipoCategoria
            };
        }
        public List<RetornoCategoriaDbo> ParseList(List<CategoriaDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
        public List<CategoriaDbo> ParseList(List<RetornoCategoriaDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(Parse).ToList();
        }
    }
}