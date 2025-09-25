using Projeto_Gabriel.Application.Hypermedia.Enricher;
using Projeto_Gabriel.Application.Hypermedia.Filters;
using Microsoft.Extensions.DependencyInjection;
using Projeto_Gabriel.Application.Hypermedia.Enricher.Financas;
using Projeto_Gabriel.Application.Hypermedia.Enricher.Contatos;

namespace Projeto_Gabriel.Application.Extensions
{
    public static class EnricherCollectionExtensions
    {
        public static IServiceCollection AddEnrichers(this IServiceCollection services, HyperMediaFilterOptions filterOptions)
        {
            // Adicionando os Enrichers existentes
            filterOptions.ContentResponseEnricherList.Add(new PessoaEnricher());
            filterOptions.ContentResponseEnricherList.Add(new LivroEnricher());
            filterOptions.ContentResponseEnricherList.Add(new PokemonEnricher());

            //Adicionando os Financas
            filterOptions.ContentResponseEnricherList.Add(new CartaoEnricher());
            filterOptions.ContentResponseEnricherList.Add(new CategoriaEnricher());
            filterOptions.ContentResponseEnricherList.Add(new LancamentoEnricher());
            filterOptions.ContentResponseEnricherList.Add(new ParcelamentoEnricher());
            filterOptions.ContentResponseEnricherList.Add(new ParcelamentoMensalEnricher());
            filterOptions.ContentResponseEnricherList.Add(new PessoaContaEnricher());

            //Adicionando Contato
            filterOptions.ContentResponseEnricherList.Add(new ContatoEnricher());

            return services;
        }
    }
}