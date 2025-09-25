using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.Hypermedia.Constants;
using System.Text;
using Projeto_Gabriel.Application.Dto;

namespace Projeto_Gabriel.Application.Hypermedia.Enricher
{
    public class LivroEnricher : ContentResponseEnricher<LivrosDbo>
    {
        protected override Task EnrichModel(LivrosDbo content, IUrlHelper urlHelper)
        {
            var path ="api/Livro";
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
                Action = HttpActionVerb.POST,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PATCH,
                Href = GetLink(content.Id, urlHelper, $"{path}/ativar"),
                Rel = "ativar",
                Type = ResponseTypeFormat.DefaultPatch
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PATCH,
                Href = GetLink(content.Id, urlHelper, $"{path}/desativar"),
                Rel = "desativar",
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
                Href = GetLink(content.Id, urlHelper, $"{path}/autor/{content.Autor}"),
                Rel = "autor",
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = GetLink(content.Id, urlHelper, $"{path}/titulo/{content.Titulo}"),
                Rel = "titulo",
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = GetLink(content.Id, urlHelper, $"{path}/dataLancamento/{content.DataLancamento:yyyy-MM-dd}"),
                Rel = "dataLancamento",
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = GetLink(content.Id, urlHelper, $"{path}/preco/{content.Preco}"),
                Rel = "preco",
                Type = ResponseTypeFormat.DefaultGet
            });

            return Task.CompletedTask;
        }

        private string GetLink(long id, IUrlHelper urlHelper, string path)
        {
            lock (this)
            {
                {
                    var url = new { controller = path, id };
                    return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F", "/").ToString();
                }
            }
        }
    }
}
