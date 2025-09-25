using Projeto_Gabriel.Application.Hypermedia;
using Projeto_Gabriel.Application.Hypermedia.Abstract;

namespace Projeto_Gabriel.Application.Dto
{
    public class CartaPokemonDbo : ISupportHyperMedia
    {
        public long Id { get; set; }
        public string NomeVersao { get; set; }
        public string Versao { get; set; }
        public int NumeroCarta { get; set; }
        public string NomePokemon { get; set; }
        public string Raridade { get; set; }
        public string Tipo { get; set; }
        public int? HP { get; set; }
        public string Estagio { get; set; }
        public string Booster { get; set; }
        public byte[] Imagem { get; set; }
        public string Situacao { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();

    }
}