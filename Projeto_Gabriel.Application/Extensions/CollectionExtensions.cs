using Projeto_Gabriel.Bussines.Implementacoes;
using Projeto_Gabriel.Bussines;
using Projeto_Gabriel.Domain.ServiceInterface;
using Projeto_Gabriel.Domain.Service;
using Microsoft.Extensions.DependencyInjection;
using Projeto_Gabriel.Infrastructure.Services.Validation;
using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Application.BusinessInterface;
using Projeto_Gabriel.Application.Business;
using Projeto_Gabriel.Domain.Service.Financas;
using Projeto_Gabriel.Domain.Entity.Financas;
using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.Business.Financas;
using Projeto_Gabriel.Domain.Entity.Logas;
using Projeto_Gabriel.Domain.Service.Logas;
using Projeto_Gabriel.Application.Business.Logas;
using Projeto_Gabriel.Application.BusinessInterface.Logas;
using Projeto_Gabriel.Domain.Entity.Contatos;
using Projeto_Gabriel.Domain.Service.Contatos;
using Projeto_Gabriel.Application.BusinessInterface.Contato;
using Projeto_Gabriel.Application.Business.Contato;


namespace Projeto_Gabriel.Application.Extensions
{
    public static class CollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IEntityValidationService<Livros>, LivrosValidationService>();
            services.AddTransient<IEntityValidationService<Usuario>, UsuarioValidationService>();
            services.AddTransient<IEntityValidationService<Pessoa>, PessoaValidationService>();
            services.AddTransient<IEntityValidationService<CartaPokemon>, CartaPokemonValidationService>();

            //Financas
            services.AddTransient<IEntityValidationService<Categoria>, CategoriaValidationService>();
            services.AddTransient<IEntityValidationService<Lancamento>, LancamentoValidationService>();
            services.AddTransient<IEntityValidationService<Cartao>, CartaoValidationService>();
            services.AddTransient<IEntityValidationService<Parcelamento>, ParcelamentoValidationService>();
            services.AddTransient<IEntityValidationService<ParcelamentoMensal>, ParcelamentoMensalValidationService>();
            services.AddTransient<IEntityValidationService<PessoaConta>, PessoaContaValidationService>();

            //Contato
            services.AddTransient<IEntityValidationService<Contato>, ContatoValidationService>();

            //Logs
            services.AddTransient<IEntityValidationService<LogEntry>, LogsValidationService>();

            return services;
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            //Curso
            services.AddScoped<IPessoaBussines, PessoaBussinesImplementacao>();
            services.AddScoped<ILivroBussines, LivroBussinesImplementacao>();

            //Login
            services.AddScoped<ILoginBussines, LoginBussinesImplementation>();
            services.AddScoped<IUsuarioBusiness, UsuarioBusinessImplementacao>();

            //Contato
            services.AddScoped<IContatoBusiness, ContatoBusinessImplementacao>();

            //Pokemon
            services.AddScoped<IPokemonFileBusiness, PokemonFileBusinessImplementacao>();
            services.AddScoped<IFileBusiness, FileBusinessImplementacao>();

            //Logs
            services.AddScoped<ILogBusiness, LogBusinessImplementacao>();

            //Financas
            services.AddScoped<ICartaoBusiness, CartaoBusinessImplementacao>();
            services.AddScoped<ICategoriaBusiness, CategoriaBusinessImplementacao>();
            services.AddScoped<IPessoaContaBusiness, PessoaContaBusinessImplementacao>();
            services.AddScoped<ILancamentoBusiness, LancamentoBusinessImplementacao>();
            services.AddScoped<IParcelamentoBusiness, ParcelamentoBusinessImplementacao>();
            services.AddScoped<IParcelamentoMensalBusiness, ParcelamentoMensalBusinessImplementacao>();

            return services;
        }
    }
}