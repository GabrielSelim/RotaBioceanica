using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Application.Hypermedia.Constants;
using System.Text;

namespace Projeto_Gabriel.Application.Hypermedia.Enricher
{
    public class PessoaEnricher : ContentResponseEnricher<PessoaDbo>
    {
        protected override Task EnrichModel(PessoaDbo content, IUrlHelper urlHelper)
        {
            var path ="api/Pessoa";
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
