using System.ComponentModel.DataAnnotations.Schema;
using Projeto_Gabriel.Domain.Entity.Base;

namespace Projeto_Gabriel.Domain.Entity
{
    [Table("pessoas")]
    public class Pessoa : BaseEntity
    {
        [Column("first_name")]
        public string PrimeiroNome { get; set; }

        [Column("last_name")]
        public string Sobrenome { get; set; }

        [Column("address")]
        public string Endereco { get; set; }
        
        [Column("gender")]
        public string Sexo { get; set; }

        [Column("enabled")]
        public bool Ativo { get; set; }

    }
}