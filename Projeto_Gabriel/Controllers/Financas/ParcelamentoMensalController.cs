using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.BusinessInterface.Logas;
using Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoMensalDbo;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Application.Hypermedia.Filters;
using Projeto_Gabriel.Application.Utils;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;

namespace Projeto_Gabriel.Controllers.Financas
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class ParcelamentoMensalController : ControllerBase
    {
        private readonly ILogger<ParcelamentoMensalController> _logger;
        private IParcelamentoMensalBusiness _parcelamentoMensalBusiness;
        private readonly ILogBusiness _logBusiness;
        public ParcelamentoMensalController(ILogger<ParcelamentoMensalController> logger, IParcelamentoMensalBusiness parcelamentoMensalBusiness, ILogBusiness logBusiness)
        {
            _logger = logger;
            _parcelamentoMensalBusiness = parcelamentoMensalBusiness;
            _logBusiness = logBusiness;
        }
        private long GetUsuarioId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub") ?? User.FindFirst("id");
            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            return userId;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(RetornoParcelamentoMensalDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoMensalBusiness.ObterPorId(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Information", $"Consulta de parcelamento mensal {id} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Parcelamento mensal {id} não encontrado.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Requisição inválida para parcelamento mensal {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Error", $"Erro inesperado ao consultar parcelamento mensal {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet]
        [ProducesResponseType((200), Type = typeof(List<RetornoParcelamentoMensalDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoMensalBusiness.ObterTodos(usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Information", $"Consulta de parcelamentos mensais realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Nenhum parcelamento mensal encontrado.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Requisição inválida para parcelamentos mensais.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Error", $"Erro inesperado ao consultar parcelamentos mensais.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("parcelamento/{parcelamentoId:long}")]
        [ProducesResponseType((200), Type = typeof(List<RetornoParcelamentoMensalDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByParcelamentoId(long parcelamentoId)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoMensalBusiness.ObterPorParcelamentoId(parcelamentoId, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Information", $"Consulta de parcelas mensais por parcelamentoId {parcelamentoId} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Nenhuma parcela mensal encontrada para parcelamentoId {parcelamentoId}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Requisição inválida para parcelas mensais por parcelamentoId {parcelamentoId}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Error", $"Erro inesperado ao consultar parcelas mensais por parcelamentoId {parcelamentoId}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("situacao/{situacao}")]
        [ProducesResponseType((200), Type = typeof(List<RetornoParcelamentoMensalDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetBySituacao(string situacao)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoMensalBusiness.ObterPorSituacao(situacao, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Information", $"Consulta de parcelas mensais por situação '{situacao}' realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Nenhuma parcela mensal encontrada para situação '{situacao}'.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Requisição inválida para parcelas mensais por situação '{situacao}'.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Error", $"Erro inesperado ao consultar parcelas mensais por situação '{situacao}'.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("pessoaconta/{pessoaContaId:long}")]
        [ProducesResponseType((200), Type = typeof(List<ParcelamentoMensalDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByPessoaContaId(long pessoaContaId)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoMensalBusiness.ObterPorPessoaContaId(pessoaContaId, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Information", $"Consulta de parcelas mensais por pessoaContaId {pessoaContaId} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Nenhuma parcela mensal encontrada para pessoaContaId {pessoaContaId}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Requisição inválida para parcelas mensais por pessoaContaId {pessoaContaId}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Error", $"Erro inesperado ao consultar parcelas mensais por pessoaContaId {pessoaContaId}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("cartao/{cartaoId:long}")]
        [ProducesResponseType((200), Type = typeof(List<ParcelamentoMensalDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByCartaoId(long cartaoId)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoMensalBusiness.ObterPorCartaoId(cartaoId, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Information", $"Consulta de parcelas mensais por cartaoId {cartaoId} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Nenhuma parcela mensal encontrada para cartaoId {cartaoId}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Requisição inválida para parcelas mensais por cartaoId {cartaoId}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Error", $"Erro inesperado ao consultar parcelas mensais por cartaoId {cartaoId}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPatch("{id:long}/pagar")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult MarcarComoPago([FromForm] MarcarComoPagoRequest request)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                _parcelamentoMensalBusiness.MarcarComoPago(request, usuarioId);

                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Information", $"Parcela mensal {request.Id} marcada como paga.");
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Erro ao marcar parcela mensal {request.Id} como paga.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Error", $"Erro inesperado ao marcar parcela mensal {request.Id} como paga.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPatch("{id:long}/inativar")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult MarcarComoInativo(long id)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                _parcelamentoMensalBusiness.MarcarComoInativo(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Information", $"Parcela mensal {id} marcada como inativa.");
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Warning", $"Erro ao marcar parcela mensal {id} como inativa.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoMensalController), "Error", $"Erro inesperado ao marcar parcela mensal {id} como inativa.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }
    }
}