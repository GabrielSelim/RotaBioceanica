using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto.Logas;
using Projeto_Gabriel.Domain.Entity.Logas;

namespace Projeto_Gabriel.Application.Converter.Implementacao.Logas
{
    public class LogEntryConverter : IParser<LogEntryDbo, LogEntry>, IParser<LogEntry, LogEntryDbo>
    {
        public LogEntry Parse(LogEntryDbo origem)
        {
            if (origem == null) return null;
            return new LogEntry
            {
                Id = origem.Id,
                DataHora = origem.DataHora,
                Nivel = origem.Nivel,
                Mensagem = origem.Mensagem,
                UserId = origem.UserId,
                NomeUsuario = origem.NomeUsuario,
                Detalhes = origem.Detalhes,
                Origem = origem.Origem
            };
        }

        public LogEntryDbo Parse(LogEntry origem)
        {
            if (origem == null) return null;
            return new LogEntryDbo
            {
                Id = origem.Id,
                DataHora = origem.DataHora,
                Nivel = origem.Nivel,
                Mensagem = origem.Mensagem,
                UserId = origem.UserId,
                NomeUsuario = origem.NomeUsuario,
                Detalhes = origem.Detalhes,
                Origem = origem.Origem
            };
        }

        public List<LogEntry> ParseList(List<LogEntryDbo> origem)
        {
            if (origem == null) return null;
            return origem.Select(item => Parse(item)).ToList();
        }

        public List<LogEntryDbo> ParseList(List<LogEntry> origem)
        {
            if (origem == null) return null;
            return origem.Select(item => Parse(item)).ToList();
        }
    }
}