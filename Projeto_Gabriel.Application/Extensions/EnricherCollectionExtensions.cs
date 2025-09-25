using Projeto_Gabriel.Application.Hypermedia.Enricher;
using Projeto_Gabriel.Application.Hypermedia.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Projeto_Gabriel.Application.Extensions
{
    public static class EnricherCollectionExtensions
    {
        public static IServiceCollection AddEnrichers(this IServiceCollection services, HyperMediaFilterOptions filterOptions)
        {
            // Adicionando os Enrichers existentes
            filterOptions.ContentResponseEnricherList.Add(new LivroEnricher());

            return services;
        }
    }
}