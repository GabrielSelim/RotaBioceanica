using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.BusinessInterface.Logas;
using Projeto_Gabriel.Application.Dto.Financas.CategoriaDbo;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Application.Hypermedia.Filters;
using Projeto_Gabriel.Application.Utils;
using Swashbuckle.AspNetCore.Filters;

namespace Projeto_Gabriel.Controllers.Financas
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ILogger<CategoriaController> _logger;
        private readonly ICategoriaBusiness _categoriaBusiness;
        private readonly ILogBusiness _logBusiness;

        public CategoriaController(ILogger<CategoriaController> logger, ICategoriaBusiness categoriaBusiness, ILogBusiness logBusiness)
        {
            _logger = logger;
            _categoriaBusiness = categoriaBusiness;
            _logBusiness = logBusiness;
        }

        private long GetUsuarioId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) ?? User.FindFirst("sub") ?? User.FindFirst("id");
            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            return userId;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(RetornoCategoriaDbo))]
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

                var categoria = _categoriaBusiness.ObterPorId(id, usuarioId);

                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Information", $"Categoria {id} consultada com sucesso.");
                return Ok(categoria);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", $"Categoria {id} não encontrada.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", $"Requisição inválida para categoria {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Error", $"Erro inesperado ao consultar categoria {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet]
        [ProducesResponseType((200), Type = typeof(List<RetornoCategoriaDbo>))]
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
                var categorias = _categoriaBusiness.ObterTodos(usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Information", "Consulta de todas as categorias realizada com sucesso.");

                return Ok(categorias);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", "Nenhuma categoria encontrada.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", "Requisição inválida ao buscar categorias.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Error", "Erro inesperado ao buscar categorias.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPost]
        [ProducesResponseType((200), Type = typeof(RetornoCategoriaDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] CriarCategoriaDbo categoria)
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

                var categoriaCriada = _categoriaBusiness.Criar(categoria, usuarioId);

                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Information", $"Categoria criada com sucesso. Nome: {categoriaCriada.NomeCategoria}");
                return Ok(categoriaCriada);
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", "Requisição inválida ao criar categoria.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Error", "Erro inesperado ao criar categoria.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("nome/{nomeCategoria}")]
        [ProducesResponseType((200), Type = typeof(List<RetornoCategoriaDbo>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByNome(string nomeCategoria)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var categorias = _categoriaBusiness.ObterCategoriasPorNome(nomeCategoria, usuarioId);

                if (categorias == null || categorias.Count == 0)
                {
                    LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", $"Nenhuma categoria encontrada com o nome '{nomeCategoria}'.");
                    throw new NotFoundException($"Nenhuma categoria encontrada com o nome '{nomeCategoria}'.");
                }

                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Information", $"Consulta de categorias por nome '{nomeCategoria}' realizada com sucesso.");
                return Ok(categorias);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", $"Nenhuma categoria encontrada com o nome '{nomeCategoria}'.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Error", $"Erro ao buscar categorias por nome '{nomeCategoria}'.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("tipo/{tipoCategoria}")]
        [ProducesResponseType((200), Type = typeof(List<CategoriaDbo>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByTipo(string tipoCategoria)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var categorias = _categoriaBusiness.ObterCategoriasPorTipo(tipoCategoria, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Information", $"Consulta de categorias por tipo '{tipoCategoria}' realizada com sucesso.");
                if (categorias == null || categorias.Count == 0)
                {
                    LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", $"Nenhuma categoria encontrada com o tipo '{tipoCategoria}'.");
                    throw new NotFoundException($"Nenhuma categoria encontrada com o tipo '{tipoCategoria}'.");
                }

                return Ok(categorias);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", $"Nenhuma categoria encontrada com o tipo '{tipoCategoria}'.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Error", $"Erro ao buscar categorias por tipo '{tipoCategoria}'.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((200), Type = typeof(void))]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete(long id)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                _categoriaBusiness.DeletarCategoria(id, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Information", $"Categoria {id} deletada com sucesso.");

                return Ok();
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", $"Categoria {id} não encontrada para deleção.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", $"Requisição inválida ao deletar categoria {id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Error", $"Erro inesperado ao deletar categoria {id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPatch]
        [ProducesResponseType((200), Type = typeof(RetornoCategoriaDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Patch([FromBody] AtualizarCategoriaDbo categoria)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var categoriaAtualizada = _categoriaBusiness.AtualizarCategoria(categoria, usuarioId);
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Information", $"Categoria {categoria.Id} atualizada com sucesso.");

                return Ok(categoriaAtualizada);
            }
            catch (NotFoundException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", $"Categoria {categoria.Id} não encontrada para atualização.", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Warning", $"Requisição inválida ao atualizar categoria {categoria.Id}.", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                LogHelper.RegistrarLog(_logBusiness, HttpContext, nameof(CategoriaController), "Error", $"Erro inesperado ao atualizar categoria {categoria.Id}.", ex.ToString());
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }
    }
}