using Projeto_Gabriel.Application.Dto.Logas;

namespace Projeto_Gabriel.Application.BusinessInterface.Logas
{
    public interface ILogBusiness
    {
        void Criar(LogEntryDbo logEntry);
    }
}