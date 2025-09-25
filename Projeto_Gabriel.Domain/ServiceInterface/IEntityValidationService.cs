namespace Projeto_Gabriel.Domain.ServiceInterface
{
    public interface IEntityValidationService<T>
    {
        void Validate(T entity);
    }
}