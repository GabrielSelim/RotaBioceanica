namespace Projeto_Gabriel.Application.Dto.Contatos
{
    public class ContatoDbo
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Mensagem { get; set; }

        public DateTime DataContato { get; set; }

        public long? UsuarioId { get; set; }
    }
}