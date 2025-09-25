using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Application.Dto.Financas.CategoriaDbo
{
    public class AtualizarCategoriaDbo
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string NomeCategoria { get; set; }

        [Required]
        public string TipoCategoria { get; set; }
    }
}