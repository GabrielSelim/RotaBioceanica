using System.Text.Json.Serialization;

namespace Projeto_Gabriel.Application.Dto.Financas.CategoriaDbo
{
    public class CategoriaDbo
    {
        public long Id { get; set; }

        [JsonIgnore]
        public long UsuarioId { get; set; }

        public string NomeCategoria { get; set; }

        public string TipoCategoria { get; set; }
    }
}