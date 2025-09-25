using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.Dto.Financas.CategoriaDbo;
using Projeto_Gabriel.Application.Hypermedia.Constants;
using System.Text;

namespace Projeto_Gabriel.Application.Hypermedia.Enricher.Financas
{
    public class CategoriaEnricher : ContentResponseEnricher<RetornoCategoriaDbo>
    {
        protected override Task EnrichModel(RetornoCategoriaDbo content, IUrlHelper urlHelper)
        {
            var path = "api/Categoria";
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
                Action = HttpActionVerb.POST,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = GetLink(content.Id, urlHelper, $"{path}/nome/{content.NomeCategoria}"),
                Rel = "nomeCategoria",
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = GetLink(content.Id, urlHelper, $"{path}/tipo/{content.TipoCategoria}"),
                Rel = "tipoCategoria",
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PATCH,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPatch
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.DELETE,
                Href = link,
                Rel = RelationType.self,
                Type = "int"
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