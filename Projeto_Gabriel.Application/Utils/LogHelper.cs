using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Projeto_Gabriel.Application.BusinessInterface.Logas;
using Projeto_Gabriel.Application.Dto.Logas;

namespace Projeto_Gabriel.Application.Utils
{
    public static class LogHelper
    {
        public static void RegistrarLog(
            ILogBusiness logBusiness,
            HttpContext httpContext,
            string origem,
            string nivel,
            string mensagem,
            string? detalhes = null)
        {
            var userId = httpContext?.User?.Identity?.IsAuthenticated == true
                ? long.TryParse(httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0
                : 0;
            var nomeUsuario = httpContext?.User?.Identity?.IsAuthenticated == true
                ? httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value
                : "Anônimo";

            logBusiness.Criar(new LogEntryDbo
            {
                DataHora = DateTime.UtcNow,
                Nivel = nivel,
                Mensagem = mensagem,
                UserId = userId,
                NomeUsuario = nomeUsuario ?? "",
                Detalhes = detalhes,
                Origem = origem
            });
        }
    }
}