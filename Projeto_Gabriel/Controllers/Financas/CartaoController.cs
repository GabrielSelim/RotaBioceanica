using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.BusinessInterface.Logas;
using Projeto_Gabriel.Application.Dto.Financas.CartaoDbo;
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
    public class CartaoController : ControllerBase
    {
        private readonly ILogger<CartaoController> _logger;
        private ICartaoBusiness _cartaoBusiness;
        private readonly ILogBusiness _logBusiness;

        public CartaoController(ILogger<CartaoController> logger, ICartaoBusiness cartaoBusiness, ILogBusiness logBusiness)
        {
            _logger = logger;
            _cartaoBusiness = cartaoBusiness;
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
        [ProducesResponseType((200), Type = typeof(RetornoCartaoDbo))]
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
                var usuarioId = GetUsuarioId();
                var cartao = _cartaoBusiness.ObterPorId(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Information", $"Consulta de cartão {id} realizada com sucesso.");
                return Ok(cartao);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Cartão {id} não encontrado.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Requisição inválida para cartão {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Error", $"Erro inesperado ao consultar cartão {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet]
        [ProducesResponseType((200), Type = typeof(List<RetornoCartaoDbo>))]
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
                var usuarioId = GetUsuarioId();
                var cartoes = _cartaoBusiness.ObterTodos(usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Information", "Consulta de todos os cartões realizada com sucesso.");
                return Ok(cartoes);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", "Nenhum cartão encontrado.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", "Requisição inválida ao buscar cartões.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Error", "Erro inesperado ao buscar cartões.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPost()]
        [ProducesResponseType((200), Type = typeof(RetornoCartaoDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] CriarCartaoDbo cartao)
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

                var criarCartao = _cartaoBusiness.Criar(cartao, usuarioId);

                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Information", $"Cartão criado com sucesso. NomeUsuario: {cartao.NomeUsuario}");
                return Ok(criarCartao);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", "Erro ao criar cartão.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", "Requisição inválida ao criar cartão.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Error", "Erro inesperado ao criar cartão.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("usuario/{nomeUsuario}")]
        [ProducesResponseType((200), Type = typeof(List<RetornoCartaoDbo>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetCartoesPorNomeUsuario(string nomeUsuario)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var cartoes = _cartaoBusiness.ObterCartoesPorNomeUsuario(nomeUsuario, usuarioId);

                if (cartoes == null || cartoes.Count == 0)
                {
                    LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Nenhum cartão encontrado para o usuário {nomeUsuario}.");
                    return NotFound(new { message = "Nenhum cartão encontrado para o usuário especificado." });
                }

                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Information", $"Consulta de cartões por usuário {nomeUsuario} realizada com sucesso.");
                return Ok(cartoes);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Erro ao buscar cartões por usuário {nomeUsuario}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Requisição inválida ao buscar cartões por usuário {nomeUsuario}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Error", $"Erro inesperado ao buscar cartões por usuário {nomeUsuario}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}"});
            }
        }

        [HttpGet("banco/{nomeBanco}")]
        [ProducesResponseType((200), Type = typeof(List<RetornoCartaoDbo>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetCartoesPorNomeBanco(string nomeBanco)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var cartoes = _cartaoBusiness.ObterCartoesPorNomeBanco(nomeBanco, usuarioId);

                if (cartoes == null || cartoes.Count == 0)
                {
                    LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Nenhum cartão encontrado para o banco {nomeBanco}.");
                    return NotFound(new { message = "Nenhum cartão encontrado para o banco especificado." });
                }

                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Information", $"Consulta de cartões por banco {nomeBanco} realizada com sucesso.");
                return Ok(cartoes);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Erro ao buscar cartões por banco {nomeBanco}.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Requisição inválida ao buscar cartões por banco {nomeBanco}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Error", $"Erro inesperado ao buscar cartões por banco {nomeBanco}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPatch]
        [ProducesResponseType((200), Type = typeof(RetornoCartaoDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Patch([FromBody] AtualizarCartaoDbo cartao)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var cartaoAtualizado = _cartaoBusiness.AtualizarCartao(cartao, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Information", $"Cartão {cartao.Id} atualizado com sucesso (PATCH).");
                return Ok(cartaoAtualizado);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Cartão {cartao.Id} não encontrado para atualização.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Requisição inválida ao atualizar cartão {cartao.Id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Error", $"Erro inesperado ao atualizar cartão {cartao.Id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
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
                _cartaoBusiness.DeletarCartao(id, usuarioId);

                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Information", $"Cartão {id} deletado com sucesso.");
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Cartão {id} não encontrado para deleção.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Warning", $"Requisição inválida ao deletar cartão {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CartaoController), "Error", $"Erro inesperado ao deletar cartão {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }
    }
}