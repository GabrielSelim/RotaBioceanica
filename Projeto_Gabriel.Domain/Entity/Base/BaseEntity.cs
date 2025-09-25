using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Gabriel.Domain.Entity.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
