using Microsoft.Extensions.DependencyInjection;
using Projeto_Gabriel.Domain.RepositoryInterface;
using Projeto_Gabriel.Domain.RepositoryInterface.Contatos;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;
using Projeto_Gabriel.Domain.RepositoryInterface.Logas;
using Projeto_Gabriel.Infrastructure.Repositorys;
using Projeto_Gabriel.Infrastructure.Repositorys.Contatos;
using Projeto_Gabriel.Infrastructure.Repositorys.Financas;
using Projeto_Gabriel.Infrastructure.Repositorys.Logas;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<ILivroRepository, LivroRepository>();
            services.AddScoped<ICartaPokemonRepository, CartaPokemonRepository>();

            //Financas
            services.AddScoped<ICartaoRepository, CartaoRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IPessoaContaRepository, PessoaContaRepository>();
            services.AddScoped<ILancamentoRepository, LancamentoRepository>();
            services.AddScoped<IParcelamentoRepository, ParcelamentoRepository>();
            services.AddScoped<IParcelamentoMensalRepository, ParcelamentoMensalRepository>();

            //Contato
            services.AddScoped<IContatoRepository, ContatoRepository>();

            //Logs
            services.AddScoped<ILogRepository, LogRepository>();

            // Registro genérico
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}