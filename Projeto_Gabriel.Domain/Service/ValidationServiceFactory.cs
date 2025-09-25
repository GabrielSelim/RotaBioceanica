using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.Entity.Contatos;
using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Domain.Entity.Logas;
using Projeto_Gabriel.Domain.Service.Contatos;
using Projeto_Gabriel.Domain.Service.Financas;
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

            if (typeof(T) == typeof(Pessoa))
                return (IEntityValidationService<T>)new PessoaValidationService();

            if (typeof(T) == typeof(CartaPokemon))
                return (IEntityValidationService<T>)new CartaPokemonValidationService();

            if (typeof(T) == typeof(Cartao))
                return (IEntityValidationService<T>)new CartaoValidationService();

            if (typeof(T) == typeof(Categoria))
                return (IEntityValidationService<T>)new CategoriaValidationService();

            if (typeof(T) == typeof(Lancamento))
                return (IEntityValidationService<T>)new LancamentoValidationService();

            if (typeof(T) == typeof(Parcelamento))
                return (IEntityValidationService<T>)new ParcelamentoValidationService();

            if (typeof(T) == typeof(ParcelamentoMensal))
                return (IEntityValidationService<T>)new ParcelamentoMensalValidationService();

            if (typeof(T) == typeof(PessoaConta))
                return (IEntityValidationService<T>)new PessoaContaValidationService();

            if (typeof(T) == typeof(Contato))
                return (IEntityValidationService<T>)new ContatoValidationService();

            if (typeof(T) == typeof(LogEntry))
                return (IEntityValidationService<T>)new LogsValidationService();

            throw new NotImplementedException($"Serviço de validação não implementado para o tipo {typeof(T).Name}");
        }
    }
}