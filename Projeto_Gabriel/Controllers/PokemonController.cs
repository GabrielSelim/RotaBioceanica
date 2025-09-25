using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Application.Hypermedia.Filters;
using Projeto_Gabriel.Application.Utils;
using Projeto_Gabriel.Bussines;
using Swashbuckle.AspNetCore.Filters;

namespace Projeto_Gabriel.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonFileBusiness _pokemonFileBusiness;

        public PokemonController(IPokemonFileBusiness pokemonFileBusiness)
        {
            _pokemonFileBusiness = pokemonFileBusiness;
        }

        [HttpPost("salvarImagensPokemon")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(200, Type = typeof(List<CartaPokemonDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        [Produces("application/json")]
        public async Task<IActionResult> SalvarImagensPokemon([FromForm] List<IFormFile> files)
        {
            try
            {
                var result = await _pokemonFileBusiness.SavePokemonImagesToDatabase(files);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém os Pokemon por Critérios a serem Preenchidos. (Endpoint liberado, não requer autenticação)
        /// </summary>
        [HttpGet("filtrarPorCriterios")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<CartaPokemonDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult FiltrarPorCriterios(
            [FromQuery] string? nomepokemon = null,
            [FromQuery] string? tipo = null,
            [FromQuery] string? raridade = null,
            [FromQuery] string? estagio = null,
            [FromQuery] string? versao = null,
            [FromQuery] string? booster = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortDirection = "asc")
        {
            try
            {
                var result = _pokemonFileBusiness.FiltrarPorCriterios(nomepokemon, tipo, raridade, estagio, versao, booster, sortField, sortDirection);

                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém os Pokemon sem suas Imagens. (Endpoint liberado, não requer autenticação)
        /// </summary>
        [HttpGet("obterTodosSemImagem")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<CartaPokemonDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult ObterTodosSemImagem()
        {
            try
            {
                var result = _pokemonFileBusiness.ObterTodosSemImagem();
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPatch("ativar/{id:long}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult AtivarCarta(long id)
        {
            try
            {
                _pokemonFileBusiness.AtivarCarta(id);
                return Ok(new { message = "Carta ativada com sucesso." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPatch("inativar/{id:long}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult InativarCarta(long id)
        {
            try
            {
                _pokemonFileBusiness.InativarCarta(id);
                return Ok(new { message = "Carta inativada com sucesso." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CartaPokemonDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult ObterPorId(long id)
        {
            try
            {
                var result = _pokemonFileBusiness.ObterPorId(id);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType((200), Type = typeof(CartaPokemonDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] CartaPokemonDbo cartaPokemon)
        {
            try
            {
                if (cartaPokemon == null)
                    return BadRequest();

                var result = _pokemonFileBusiness.Atualizar(cartaPokemon);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Deletar(long id)
        {
            try
            {
                _pokemonFileBusiness.Deletar(id);

                return Ok(new { message = "Carta Deletada com sucesso." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}