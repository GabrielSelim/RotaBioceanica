using Projeto_Gabriel.Domain.Entity.Logas;
using Projeto_Gabriel.Domain.RepositoryInterface.Logas;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Infrastructure.Repositorys.Logas
{
    public class LogRepository : GenericRepository<LogEntry>, ILogRepository
    {
        public LogRepository(MySQLContext context) : base(context)
        {
        }
    }
}
