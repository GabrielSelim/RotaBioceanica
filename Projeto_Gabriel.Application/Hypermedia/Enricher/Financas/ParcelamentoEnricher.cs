using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoDbo;
using Projeto_Gabriel.Application.Hypermedia.Constants;
using System.Text;

namespace Projeto_Gabriel.Application.Hypermedia.Enricher.Financas
{
    public class ParcelamentoEnricher : ContentResponseEnricher<RetornoParcelamentoDbo>
    {
        protected override Task EnrichModel(RetornoParcelamentoDbo content, IUrlHelper urlHelper)
        {
            var path = "api/Parcelamento";
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
                Href = GetLink(content.Id, urlHelper, $"{path}/{content.Id}/mensais"),
                Rel = "parcelasMensais",
                Type = ResponseTypeFormat.DefaultGet
            });
            if (content.PessoaContaId.HasValue)
            {
                content.Links.Add(new HyperMediaLink()
                {
                    Action = HttpActionVerb.GET,
                    Href = GetLink(content.Id, urlHelper, $"{path}/pessoaconta/{content.PessoaContaId}"),
                    Rel = "porPessoaConta",
                    Type = ResponseTypeFormat.DefaultGet
                });
            }
            if (content.CartaoId.HasValue)
            {
                content.Links.Add(new HyperMediaLink()
                {
                    Action = HttpActionVerb.GET,
                    Href = GetLink(content.Id, urlHelper, $"{path}/cartao/{content.CartaoId}"),
                    Rel = "porCartao",
                    Type = ResponseTypeFormat.DefaultGet
                });
            }
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.DELETE,
                Href = link,
                Rel = RelationType.self,
                Type = "int"
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PUT,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPut
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