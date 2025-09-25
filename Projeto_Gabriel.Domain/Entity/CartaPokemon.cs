using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Projeto_Gabriel.Domain.Entity.Base;

namespace Projeto_Gabriel.Domain.Entity
{
    [Table("cartas_pokemon")]
    public class CartaPokemon : BaseEntity
    {
        [Column("nome_versao")]
        public string NomeVersao { get; set; }

        [Column("versao")]
        public string Versao { get; set; }

        [Column("numero_carta")]
        public int NumeroCarta { get; set; }

        [Column("nome_pokemon")]
        public string NomePokemon { get; set; }

        [Column("raridade")]
        public string Raridade { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        [Column("hp")]
        public int? HP { get; set; }

        [Column("estagio")]
        public string Estagio { get; set; }

        [Column("booster")]
        public string Booster { get; set; }

        [Column("imagem")]
        public byte[] Imagem { get; set; }

        [Column("situacao")]
        [StringLength(1)]
        public string Situacao { get; set; } = "A";
    }
}