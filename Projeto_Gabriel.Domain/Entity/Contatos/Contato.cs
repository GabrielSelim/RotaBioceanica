using Projeto_Gabriel.Domain.Entity.Base;

namespace Projeto_Gabriel.Domain.Entity.Contatos
{
    public class Contato : BaseEntity
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Mensagem { get; set; }

        public DateTime DataContato { get; set; }

        public long? UsuarioId { get; set; }


        /// Entity Relationships
        public Usuario? Usuario { get; set; }
    }
}