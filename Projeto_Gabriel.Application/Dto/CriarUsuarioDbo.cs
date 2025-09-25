using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Application.Dto
{
    public class CriarUsuarioDbo
    {
        [Required]
        public required string NomeUsuario { get; set; }

        [Required]
        public required string Senha { get; set; }

        [Required]
        public required string NomeCompleto { get; set; }
    }
}