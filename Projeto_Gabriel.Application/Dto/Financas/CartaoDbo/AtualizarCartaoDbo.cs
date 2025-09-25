using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Application.Dto.Financas.CartaoDbo
{
    public class AtualizarCartaoDbo
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string NomeUsuario { get; set; }

        [Required]
        public string NomeBanco { get; set; }
    }
}