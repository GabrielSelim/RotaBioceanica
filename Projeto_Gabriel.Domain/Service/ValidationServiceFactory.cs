using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.Entity.Logas;
using Projeto_Gabriel.Domain.Service.Logas;
using Projeto_Gabriel.Domain.ServiceInterface;

namespace Projeto_Gabriel.Infrastructure.Services.Validation
{
    public class ValidationServiceFactory
    {
        public static IEntityValidationService<T> GetValidationService<T>()
        {
            if (typeof(T) == typeof(Livros))
                return (IEntityValidationService<T>)new LivrosValidationService();

            if (typeof(T) == typeof(Usuario))
                return (IEntityValidationService<T>)new UsuarioValidationService();

            if (typeof(T) == typeof(LogEntry))
                return (IEntityValidationService<T>)new LogsValidationService();

            throw new NotImplementedException($"Serviço de validação não implementado para o tipo {typeof(T).Name}");
        }
    }
}