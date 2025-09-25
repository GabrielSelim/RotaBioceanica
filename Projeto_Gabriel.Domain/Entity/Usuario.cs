using Projeto_Gabriel.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Gabriel.Domain.Entity
{
    [Table("usuarios")]
    public class Usuario : BaseEntity
    {
        [Column("user_name")]
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public string NomeUsuario { get; set; }

        [Column("full_name")]
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string NomeCompleto { get; set; }

        [Column("password")]
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; }

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry_time")]
        public DateTime? RefreshTokenTempoExpiracao { get; set; }

        [Column("role")]
        public string Role { get; set; } = "User";
    }
}