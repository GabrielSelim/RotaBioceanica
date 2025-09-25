using Projeto_Gabriel.Domain.Entity.Logas;
using Projeto_Gabriel.Domain.ServiceInterface;

namespace Projeto_Gabriel.Domain.Service.Logas
{
    public class LogsValidationService : IEntityValidationService<LogEntry>
    {
        public void Validate(LogEntry logEntry)
        {
            if (logEntry == null)
                throw new ArgumentNullException(nameof(logEntry), "O log não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(logEntry.Mensagem))
                throw new ArgumentException("A mensagem do log não pode ser vazia ou nula.", nameof(logEntry.Mensagem));

            if (logEntry.DataHora == default)
                throw new ArgumentException("A data/hora do log não pode ser nula.", nameof(logEntry.DataHora));

            if (logEntry.Nivel == null)
                throw new ArgumentException("O nível de log não pode ser nulo.", nameof(logEntry.Nivel));

            if (logEntry.UserId == default)
                throw new ArgumentException("O ID do usuário não pode ser nulo.", nameof(logEntry.UserId));

            if (string.IsNullOrWhiteSpace(logEntry.NomeUsuario))
                throw new ArgumentException("O Nome do usuário não pode ser nulo.", nameof(logEntry.NomeUsuario));
        }
    }
}