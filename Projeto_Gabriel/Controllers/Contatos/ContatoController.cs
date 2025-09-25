using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.BusinessInterface.Contato;
using Projeto_Gabriel.Application.BusinessInterface.Logas;
using Projeto_Gabriel.Application.Dto.Contatos;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Application.Hypermedia.Filters;
using Projeto_Gabriel.Application.Utils;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;

namespace Projeto_Gabriel.Controllers.Contatos
{
    [ApiVersion("1")]
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly ILogger<ContatoController> _logger;
        private readonly ILogBusiness _logBusiness;
        private readonly IContatoBusiness _contatoBusiness;

        public ContatoController(ILogger<ContatoController> logger, ILogBusiness logBusiness, IContatoBusiness contatoBusiness)
        {
            _logger = logger;
            _logBusiness = logBusiness;
            _contatoBusiness = contatoBusiness;
        }

        private long? GetUsuarioId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub") ?? User.FindFirst("id");
            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
                return null;
            return userId;
        }

        [HttpGet("{id}")]
        [Authorize("Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType((200), Type = typeof(RetornoContatoDbo))]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            try
            {
                var contato = _contatoBusiness.ObterPorId(id);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Information", $"Contato obtido com sucesso: {id}");

                return Ok(contato);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Warning", $"Contato não encontrado: {id}", ex.Message);
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Warning", $"Usuário não autenticado ao tentar obter contato por ID: {id}", ex.Message);
                return Unauthorized(new ErrorResponse { Message = "Usuário não autenticado." });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Warning", $"Erro de validação ao obter contato por ID: {id}", ex.Message);
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Error", $"Erro ao obter contato por ID: {id}", ex.Message);
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet]
        [Authorize("Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType((200), Type = typeof(List<RetornoContatoDbo>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            try
            {
                var contato = _contatoBusiness.ObterTodos();
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Information", "Consulta de todos os contatos realizada com sucesso.");

                return Ok(contato);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Warning", "Nenhum contato encontrado.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Warning", "Requisição inválida ao buscar contatos.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Error", "Erro inesperado ao buscar contatos.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPost()]
        [ProducesResponseType((200), Type = typeof(RetornoContatoDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] CriarContatoDbo contato)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var criarContato = _contatoBusiness.Criar(contato, usuarioId);
                if (usuarioId != null)
                {
                    LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Information", $"Contato criado com sucesso. Nome: {contato.Nome}, Email: {contato.Email}");
                }

                return Ok(criarContato);
            }
            catch (NotFoundException ex)
            {
                if (GetUsuarioId() != null)
                {
                    LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Warning", "Erro ao criar contato.", ex.Message);
                }

                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                if (GetUsuarioId() != null)
                {
                    LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Warning", "Requisição inválida ao criar contato.", ex.Message);                    
                }

                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                if (GetUsuarioId() != null)
                {
                    LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(ContatoController), "Error", "Erro inesperado ao criar contato.", ex.ToString());
                }

                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }
    }
}