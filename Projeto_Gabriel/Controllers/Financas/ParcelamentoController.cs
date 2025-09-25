using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.BusinessInterface.Logas;
using Projeto_Gabriel.Application.Dto.Financas.ParcelamentoDbo;
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
    public class ParcelamentoController : ControllerBase
    {
        private readonly ILogger<ParcelamentoController> _logger;
        private IParcelamentoBusiness _parcelamentoBusiness;
        private readonly ILogBusiness _logBusiness;

        public ParcelamentoController(ILogger<ParcelamentoController> logger, IParcelamentoBusiness parcelamentoBusiness, ILogBusiness logBusiness)
        {
            _logger = logger;
            _parcelamentoBusiness = parcelamentoBusiness;
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
        [ProducesResponseType(typeof(RetornoParcelamentoDbo), 200)]
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
                var result = _parcelamentoBusiness.ObterPorId(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Information", $"Consulta de parcelamento {id} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Parcelamento {id} não encontrado.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Requisição inválida para parcelamento {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Error", $"Erro ao consultar parcelamento {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RetornoParcelamentoDbo>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetAll()
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoBusiness.ObterTodos(usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Information", "Consulta de todos os parcelamentos realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", "Nenhum parcelamento encontrado.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Error", "Erro ao consultar parcelamentos.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(RetornoParcelamentoDbo), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] CriarParcelamentoDbo parcelamento)
        {
            try
            {
                var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                if (claim == null)
                    claim = User.FindFirst("sub");

                if (claim == null)
                {
                    LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", "Usuário não autenticado ou claim NameIdentifier/sub ausente.");
                    return Unauthorized(new { message = "Usuário não autenticado ou claim inválida." });
                }

                var usuarioId = long.Parse(claim.Value);

                var result = _parcelamentoBusiness.Criar(parcelamento, usuarioId);

                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Information", $"Parcelamento criado com sucesso. Id: {result.Id}");
                return CreatedAtAction(nameof(Get), new { id = result.Id, version = "1" }, result);
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", "Erro de validação ao criar parcelamento.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Error", "Erro ao criar parcelamento.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPut()]
        [ProducesResponseType(typeof(RetornoParcelamentoDbo), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] AtualizarParcelamentoDbo parcelamento)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoBusiness.Atualizar(parcelamento, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Information", $"Parcelamento {parcelamento.Id} atualizado com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Parcelamento {parcelamento.Id} não encontrado para atualização.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Erro de validação ao atualizar parcelamento {parcelamento.Id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Error", $"Erro ao atualizar parcelamento {parcelamento.Id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("{id:long}/mensais")]
        [ProducesResponseType(typeof(List<ParcelamentoMensalDbo>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetMensais(long id)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoBusiness.ObterParcelamentosMensaisPorParcelamentoId(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Information", $"Consulta de parcelas mensais do parcelamento {id} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Nenhuma parcela mensal encontrada para o parcelamento {id}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Error", $"Erro ao consultar parcelas mensais do parcelamento {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("pessoaconta/{pessoaContaId:long}")]
        [ProducesResponseType(typeof(List<RetornoParcelamentoDbo>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByPessoaConta(long pessoaContaId)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoBusiness.ObterPorPessoaContaId(pessoaContaId, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Information", $"Consulta de parcelamentos por pessoaContaId {pessoaContaId} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Nenhum parcelamento encontrado para pessoaContaId {pessoaContaId}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Error", $"Erro ao consultar parcelamentos por pessoaContaId {pessoaContaId}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("cartao/{cartaoId:long}")]
        [ProducesResponseType(typeof(List<RetornoParcelamentoDbo>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByCartao(long cartaoId)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoBusiness.ObterPorCartaoId(cartaoId, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Information", $"Consulta de parcelamentos por cartaoId {cartaoId} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Nenhum parcelamento encontrado para cartaoId {cartaoId}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Error", $"Erro ao consultar parcelamentos por cartaoId {cartaoId}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("pessoaconta/{pessoaContaId:long}/cartao/{cartaoId:long}")]
        [ProducesResponseType(typeof(List<RetornoParcelamentoDbo>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByPessoaContaECartao(long pessoaContaId, long cartaoId)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var result = _parcelamentoBusiness.ObterPorPessoaContaIdECartaoId(pessoaContaId, cartaoId, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Information", $"Consulta de parcelamentos por pessoaContaId {pessoaContaId} e cartaoId {cartaoId} realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Nenhum parcelamento encontrado para pessoaContaId {pessoaContaId} e cartaoId {cartaoId}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Error", $"Erro ao consultar parcelamentos por pessoaContaId {pessoaContaId} e cartaoId {cartaoId}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("situacao/{situacao}")]
        [ProducesResponseType(typeof(List<RetornoParcelamentoDbo>), 200)]
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
                var result = _parcelamentoBusiness.ObterPorSituacao(situacao, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Information", $"Consulta de parcelamentos por situação '{situacao}' realizada com sucesso.");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Nenhum parcelamento encontrado para situação '{situacao}'.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Error", $"Erro ao consultar parcelamentos por situação '{situacao}'.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpDelete("{id:long}")]
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
                _parcelamentoBusiness.DeletarParcelamento(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Information", $"Parcelamento {id} deletado com sucesso.");
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Parcelamento {id} não encontrado para deleção.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Warning", $"Requisição inválida para deleção do parcelamento {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ParcelamentoController), "Error", $"Erro ao deletar parcelamento {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }
    }
}