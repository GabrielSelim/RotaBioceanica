using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Application.Hypermedia.Constants;
using System.Text;

namespace Projeto_Gabriel.Application.Hypermedia.Enricher
{
    public class PokemonEnricher : ContentResponseEnricher<CartaPokemonDbo>
    {
        protected override Task EnrichModel(CartaPokemonDbo content, IUrlHelper urlHelper)
        {
            var path = "api/Pokemon";
            string link = GetLink(content.Id, urlHelper, path);

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultGet
            });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PUT,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPut
            });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PATCH,
                Href = GetLink(content.Id, urlHelper, $"{path}/ativar/{content.Id}"),
                Rel = "ativar",
                Type = ResponseTypeFormat.DefaultPatch
            });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PATCH,
                Href = GetLink(content.Id, urlHelper, $"{path}/inativar/{content.Id}"),
                Rel = "inativar",
                Type = ResponseTypeFormat.DefaultPatch
            });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.DELETE,
                Href = link,
                Rel = RelationType.self,
                Type = "int"
            });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = GetLink(content.Id, urlHelper, $"{path}/filtrarPorCriterios"),
                Rel = "filtrarPorCriterios",
                Type = ResponseTypeFormat.DefaultGet
            });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = GetLink(content.Id, urlHelper, $"{path}/obterTodosSemImagem"),
                Rel = "obterTodosSemImagem",
                Type = ResponseTypeFormat.DefaultGet
            });

            return Task.CompletedTask;
        }

        private string GetLink(long id, IUrlHelper urlHelper, string path)
        {
            lock (this)
            {
                var url = new { controller = path, id };
                return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F", "/").ToString();
            }
        }
    }
}