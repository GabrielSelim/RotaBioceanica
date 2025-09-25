using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo;
using Projeto_Gabriel.Application.Hypermedia.Constants;
using System.Text;

namespace Projeto_Gabriel.Application.Hypermedia.Enricher.Financas
{
    public class LancamentoEnricher : ContentResponseEnricher<RetornoLancamentoDbo>
    {
        protected override Task EnrichModel(RetornoLancamentoDbo content, IUrlHelper urlHelper)
        {
            var path = "api/Lancamento";
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
                Href = GetLink(content.Id, urlHelper, $"{path}/categoria/{content.CategoriaId}"),
                Rel = "porCategoria",
                Type = ResponseTypeFormat.DefaultGet
            });
            if (content.ParcelamentoMensalId.HasValue)
            {
                content.Links.Add(new HyperMediaLink()
                {
                    Action = HttpActionVerb.GET,
                    Href = GetLink(content.Id, urlHelper, $"{path}/parcelamentomensal/{content.ParcelamentoMensalId}"),
                    Rel = "porParcelamentoMensal",
                    Type = ResponseTypeFormat.DefaultGet
                });
            }
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = GetLink(content.Id, urlHelper, $"{path}/situacao/{content.Situacao}"),
                Rel = "porSituacao",
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = GetLink(content.Id, urlHelper, $"{path}/periodo"),
                Rel = "porPeriodo",
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PUT,
                Href = GetLink(content.Id, urlHelper, $"{path}/{content.Id}/pagar"),
                Rel = "marcarComoPago",
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