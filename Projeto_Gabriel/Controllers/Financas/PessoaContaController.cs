using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.BusinessInterface.Logas;
using Projeto_Gabriel.Application.Dto.Financas.PessoaContaDbo;
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
    public class PessoaContaController : ControllerBase
    {
        private readonly ILogger<PessoaContaController> _logger;
        private IPessoaContaBusiness _pessoaContaBusiness;
        private readonly ILogBusiness _logBusiness;

        public PessoaContaController(ILogger<PessoaContaController> logger, IPessoaContaBusiness pessoaContaBusiness, ILogBusiness logBusiness)
        {
            _logger = logger;
            _pessoaContaBusiness = pessoaContaBusiness;
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
        [ProducesResponseType((200), Type = typeof(RetornoPessoaContaDbo))]
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
                var pessoaConta = _pessoaContaBusiness.ObterPorId(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Information", $"Consulta de pessoaConta {id} realizada com sucesso.");
                return Ok(pessoaConta);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"PessoaConta {id} não encontrada.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"Requisição inválida para pessoaConta {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Error", $"Erro inesperado ao consultar pessoaConta {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("nome/{nome}")]
        [ProducesResponseType((200), Type = typeof(List<RetornoPessoaContaDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult ObterPorNome(string nome)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var pessoas = _pessoaContaBusiness.ObterPorNome(nome, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Information", $"Consulta de pessoaConta por nome {nome} realizada com sucesso.");
                return Ok(pessoas);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"PessoaConta com nome {nome} não encontrada.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"Requisição inválida para pessoaConta com nome {nome}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Error", $"Erro inesperado ao consultar pessoaConta com nome {nome}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet]
        [ProducesResponseType((200), Type = typeof(List<RetornoPessoaContaDbo>))]
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
                var pessoas = _pessoaContaBusiness.ObterTodos(usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Information", $"Consulta de todas as pessoaContas realizada com sucesso.");
                return Ok(pessoas);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"Nenhuma pessoaConta encontrada.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"Requisição inválida para pessoaContas.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Error", $"Erro inesperado ao consultar todas as pessoaContas.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPost]
        [ProducesResponseType((200), Type = typeof(RetornoPessoaContaDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] CriarPessoaContaDbo pessoaConta)
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

                var pessoaContaCriada = _pessoaContaBusiness.Criar(pessoaConta, usuarioId);

                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Information", $"PessoaConta criada com sucesso.");

                return Ok(pessoaContaCriada);
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"Requisição inválida para criar pessoaConta.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Error", $"Erro inesperado ao criar pessoaConta.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPatch]
        [ProducesResponseType((200), Type = typeof(RetornoPessoaContaDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Patch([FromBody] AtualizarPessoaContaDbo pessoaConta)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var pessoaContaAtualizado = _pessoaContaBusiness.AtualizarPessoaConta(pessoaConta, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Information", $"PessoaConta {pessoaConta.Id} atualizada com sucesso (PATCH).");

                return Ok(pessoaContaAtualizado);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"PessoaConta {pessoaConta.Id} não encontrada para atualização.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"Requisição inválida ao atualizar pessoaConta {pessoaConta.Id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Error", $"Erro inesperado ao atualizar pessoaConta {pessoaConta.Id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((204))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete(long id)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                _pessoaContaBusiness.DeletarPessoaConta(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Information", $"PessoaConta {id} deletada com sucesso.");

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"PessoaConta {id} não encontrada para deleção.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Warning", $"Requisição inválida ao deletar pessoaConta {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(PessoaContaController), "Error", $"Erro inesperado ao deletar pessoaConta {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }
    }
}