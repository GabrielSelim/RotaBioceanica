using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.Dto.Contatos;
using Projeto_Gabriel.Application.Hypermedia.Constants;
using System;
using System.Text;

namespace Projeto_Gabriel.Application.Hypermedia.Enricher.Contatos
{
    public class ContatoEnricher : ContentResponseEnricher<RetornoContatoDbo>
    {
        protected override Task EnrichModel(RetornoContatoDbo content, IUrlHelper urlHelper)
        {
            var path = "api/Contato";
            var version = urlHelper.ActionContext.RouteData.Values["version"]?.ToString() ?? "1";
            string link = GetLink(content.Id, urlHelper, path);
            string getAllLink = urlHelper.Link("DefaultApi", new { version, controller = path });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = getAllLink,
                Rel = "all",
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.POST,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost
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