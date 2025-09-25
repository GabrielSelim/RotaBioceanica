using Projeto_Gabriel.Application.BusinessInterface.Logas;
using Projeto_Gabriel.Application.Converter.Implementacao.Logas;
using Projeto_Gabriel.Application.Dto.Logas;
using Projeto_Gabriel.Domain.RepositoryInterface.Logas;

namespace Projeto_Gabriel.Application.Business.Logas
{
    public class LogBusinessImplementacao : ILogBusiness
    {
        private readonly ILogRepository _logRepository;
        private readonly LogEntryConverter _converter;

        public LogBusinessImplementacao(ILogRepository logRepository)
        {
            _logRepository = logRepository;
            _converter = new LogEntryConverter();
        }

        public void Criar(LogEntryDbo logEntry)
        {
            try
            {
                var logEntity = _converter.Parse(logEntry);

                if (logEntity == null)
                    throw new ArgumentException("Erro ao converter o Log.");

                var logCriado = _logRepository.Criar(logEntity);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro do sistema.", ex);
            }
        }
    }
}