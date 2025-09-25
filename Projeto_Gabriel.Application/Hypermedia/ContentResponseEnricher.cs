using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Projeto_Gabriel.Application.Hypermedia.Abstract;
using Projeto_Gabriel.Application.Hypermedia.Utils;
using System.Collections.Concurrent;

namespace Projeto_Gabriel.Application.Hypermedia
{
    public abstract class ContentResponseEnricher<T> : IResponseEnricher where T : ISupportHyperMedia
    {
        public ContentResponseEnricher()
        {

        }

        public bool CanEnrich(Type contentType)
        {
            return contentType == typeof(T) || contentType == typeof(List<T>) || contentType == typeof(PagedSearchDbo<T>);
        }

        protected abstract Task EnrichModel(T content, IUrlHelper urlHelper);

        bool IResponseEnricher.CanEnrich(ResultExecutingContext response)
        {
            if (response.Result is OkObjectResult okobjectResult)
            {
                return CanEnrich(okobjectResult.Value.GetType());
            }

            return false;
        }


        public async Task Enrich(ResultExecutingContext response)
        {
            var urlHelper = new UrlHelperFactory().GetUrlHelper(response);

            if (response.Result is OkObjectResult okobjectResult)
            {
                if (okobjectResult.Value is T model)
                {
                    await EnrichModel(model, urlHelper);
                }
                else if (okobjectResult.Value is List<T> collection)
                {
                    ConcurrentBag<T> bag = new ConcurrentBag<T>(collection);
                    Parallel.ForEach(bag, (element) =>
                    {
                        EnrichModel(element, urlHelper);
                    });
                }
                else if (okobjectResult.Value is PagedSearchDbo<T> pagedSearch)
                {
                    Parallel.ForEach(pagedSearch.List.ToList(), (element) =>
                    {
                        EnrichModel(element, urlHelper);
                    });
                }

                await Task.FromResult<object>(null);
            }
        }
    }
}