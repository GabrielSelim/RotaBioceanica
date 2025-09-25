using Microsoft.Extensions.DependencyInjection;
using Projeto_Gabriel.Domain.RepositoryInterface;
using Projeto_Gabriel.Domain.RepositoryInterface.Logas;
using Projeto_Gabriel.Infrastructure.Repositorys;
using Projeto_Gabriel.Infrastructure.Repositorys.Logas;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ILivroRepository, LivroRepository>();

            //Logs
            services.AddScoped<ILogRepository, LogRepository>();

            // Registro genérico
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}