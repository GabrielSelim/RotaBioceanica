using Projeto_Gabriel.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Projeto_Gabriel.Domain.Entity.Validations;

[Table("books")]
public class Livros : BaseEntity
{
    [Column("author")]
    [MaxLength(50)]
    public string Autor { get; set; }

    [Column("launch_date")]
    public DateTime DataLancamento { get; set; }

    [Column("price")]
    [Range(0, 9999.99)]
    public decimal Preco { get; set; }

    [Column("title")]
    [MaxLength(50)]
    public string Titulo { get; set; }

    [Column("enabled")]
    public bool Ativo { get; set; }
}
