using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoMensalDbo;
using Projeto_Gabriel.Application.Hypermedia.Constants;
using System.Text;

namespace Projeto_Gabriel.Application.Hypermedia.Enricher.Financas
{
    public class ParcelamentoMensalEnricher : ContentResponseEnricher<RetornoParcelamentoMensalDbo>
    {
        protected override Task EnrichModel(RetornoParcelamentoMensalDbo content, IUrlHelper urlHelper)
        {
            var path = "api/ParcelamentoMensal";
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
                Href = GetLink(content.Id, urlHelper, $"{path}/parcelamento/{content.ParcelamentoId}"),
                Rel = "porParcelamento",
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
                Action = HttpActionVerb.GET,
                Href = GetLink(content.Id, urlHelper, $"{path}/situacao/{content.Situacao}"),
                Rel = "porSituacao",
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PUT,
                Href = GetLink(content.Id, urlHelper, $"{path}/{content.Id}/pagar"),
                Rel = "marcarComoPago",
                Type = ResponseTypeFormat.DefaultPut
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PUT,
                Href = GetLink(content.Id, urlHelper, $"{path}/{content.Id}/inativar"),
                Rel = "marcarComoInativo",
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