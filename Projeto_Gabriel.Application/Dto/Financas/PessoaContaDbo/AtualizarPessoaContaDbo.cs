using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Application.Dto.Financas.PessoaContaDbo
{
    public class AtualizarPessoaContaDbo
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string NomePessoa { get; set; }
    }
}