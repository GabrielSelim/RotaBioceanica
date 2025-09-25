using System.Text.Json.Serialization;

namespace Projeto_Gabriel.Application.Dto
{
    public class UsuarioDbo 
    {
        public string NomeUsuario { get; set; }

        public string Senha { get; set; }

        [JsonIgnore]
        public string? NomeCompleto { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        [JsonIgnore]
        public DateTime? RefreshTokenTempoExpiracao { get; set; }

        [JsonIgnore]
        public string Role { get; set; } = "User";
    }
}