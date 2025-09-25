using Projeto_Gabriel.Application.Hypermedia;
using Projeto_Gabriel.Application.Hypermedia.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Projeto_Gabriel.Application.Dto
{
    public class LivrosDbo : ISupportHyperMedia
    {
        [JsonPropertyName("code")]
        public long Id { get; set; }

        [JsonPropertyName("Escritor")]
        public string Autor { get; set; }

        public DateTime DataLancamento{ get; set; }
        
        public decimal Preco { get; set; }

        [MaxLength(100)]
        public string Titulo { get; set; }

        public bool Ativo { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}