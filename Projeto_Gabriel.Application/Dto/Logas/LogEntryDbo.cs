namespace Projeto_Gabriel.Application.Dto.Logas
{
    public class LogEntryDbo
    {
        public long Id { get; set; } // ID do log
        public DateTime DataHora { get; set; }
        public string Nivel { get; set; } // Ex: Information, Warning, Error
        public string Mensagem { get; set; }
        public long UserId { get; set; } // ID do usuário que gerou o log
        public string NomeUsuario { get; set; } // Nome do usuário que gerou o log
        public string? Detalhes { get; set; }
        public string? Origem { get; set; }
    }
}