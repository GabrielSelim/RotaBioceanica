using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.BusinessInterface.Logas;
using Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo;
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
    public class LancamentoController : ControllerBase
    {
        private readonly ILogger<LancamentoController> _logger;
        private readonly ILancamentoBusiness _lancamentoBusiness;
        private readonly ILogBusiness _logBusiness;

        public LancamentoController(ILogger<LancamentoController> logger, ILancamentoBusiness lancamentoBusiness, ILogBusiness logBusiness)
        {
            _logger = logger;
            _lancamentoBusiness = lancamentoBusiness;
            _logBusiness = logBusiness;
        }

        private long GetUsuarioId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub") ?? User.FindFirst("id");
            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            return userId;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(RetornoLancamentoDbo), 200)]
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
                var result = _lancamentoBusiness.ObterPorId(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Information", $"Consulta de lançamento {id} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Lançamento {id} não encontrado.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Requisição inválida para lançamento {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Error", $"Erro inesperado ao consultar lançamento {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RetornoLancamentoDbo>), 200)]
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
                var result = _lancamentoBusiness.ObterTodos(usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Information", $"Consulta de lançamentos realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Nenhum lançamento encontrado.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Requisição inválida para lançamentos.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Error", $"Erro inesperado ao consultar lançamentos.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("categoria/{categoriaId:long}")]
        [ProducesResponseType(typeof(List<RetornoLancamentoDbo>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByCategoriaId(long categoriaId)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _lancamentoBusiness.ObterPorCategoriaId(categoriaId, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Information", $"Consulta de lançamentos por categoriaId {categoriaId} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Nenhum lançamento encontrado para categoriaId {categoriaId}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Requisição inválida para lançamentos por categoriaId {categoriaId}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Error", $"Erro inesperado ao consultar lançamentos por categoriaId {categoriaId}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("parcelamentomensal/{parcelamentoMensalId:long}")]
        [ProducesResponseType(typeof(List<RetornoLancamentoDbo>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByParcelamentoMensalId(long parcelamentoMensalId)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _lancamentoBusiness.ObterPorParcelamentoMensalId(parcelamentoMensalId, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Information", $"Consulta de lançamentos por parcelamentoMensalId {parcelamentoMensalId} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Nenhum lançamento encontrado para parcelamentoMensalId {parcelamentoMensalId}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Requisição inválida para lançamentos por parcelamentoMensalId {parcelamentoMensalId}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Error", $"Erro inesperado ao consultar lançamentos por parcelamentoMensalId {parcelamentoMensalId}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("situacao/{situacao}")]
        [ProducesResponseType(typeof(List<RetornoLancamentoDbo>), 200)]
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
                var result = _lancamentoBusiness.ObterPorSituacao(situacao, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Information", $"Consulta de lançamentos por situação '{situacao}' realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Nenhum lançamento encontrado para situação '{situacao}'.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Requisição inválida para lançamentos por situação '{situacao}'.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Error", $"Erro inesperado ao consultar lançamentos por situação '{situacao}'.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("periodo")]
        [ProducesResponseType(typeof(List<RetornoLancamentoDbo>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _lancamentoBusiness.ObterPorPeriodo(usuarioId, dataInicio, dataFim);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Information", $"Consulta de lançamentos por período realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Nenhum lançamento encontrado para o período informado.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Requisição inválida para lançamentos por período.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Error", $"Erro inesperado ao consultar lançamentos por período.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPut("{id:long}/pagar")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        public IActionResult MarcarComoPago([FromBody] MarcarComoPagoRequest request)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                _lancamentoBusiness.MarcarComoPago(request, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Information", $"Lançamento {request.Id} marcado como pago.");
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Erro ao marcar lançamento {request.Id} como pago.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Error", $"Erro inesperado ao marcar lançamento {request.Id} como pago.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(RetornoLancamentoDbo), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] CriarLancamentoDbo lancamento)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _lancamentoBusiness.Criar(lancamento, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Information", $"Lançamento criado com sucesso. Id: {result.Id}");
                return CreatedAtAction(nameof(Get), new { id = result.Id, version = "1" }, result);
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", "Erro de validação ao criar lançamento.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Error", "Erro ao criar lançamento.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        public IActionResult Delete(long id)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                _lancamentoBusiness.Deletar(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Information", $"Lançamento {id} deletado com sucesso.");
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Lançamento {id} não encontrado para deleção.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Requisição inválida para deleção do lançamento {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Error", $"Erro ao deletar lançamento {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPatch]
        [ProducesResponseType(typeof(RetornoLancamentoDbo), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Patch([FromBody] AtualizarLancamentoDbo atualizar)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _lancamentoBusiness.Atualizar(atualizar, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Information", $"Lançamento {atualizar.Id} atualizado com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Lançamento {atualizar.Id} não encontrado para atualização.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Warning", $"Requisição inválida para atualização do lançamento {atualizar.Id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(LancamentoController), "Error", $"Erro ao atualizar lançamento {atualizar.Id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }
    }
}