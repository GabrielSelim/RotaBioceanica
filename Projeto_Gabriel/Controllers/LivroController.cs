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
    public class LivroController : ControllerBase
    {
        private readonly ILogger<LivroController> _logger;
        private ILivroBussines _livroBussines;

        public LivroController(ILogger<LivroController> logger, ILivroBussines livroBussines)
        {
            _logger = logger;
            _livroBussines = livroBussines;
        }

        /// <summary>
        /// Obtém todos os livros. (Endpoint liberado, não requer autenticação)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType((200), Type = typeof(List<LivrosDbo>))]
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
                return Ok(_livroBussines.ObterTodos());
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtém o livro por id. (Endpoint liberado, não requer autenticação)
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType((200), Type = typeof(LivrosDbo))]
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
                var livro = _livroBussines.ObterPorId(id);

                return Ok(livro);
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPatch("desativar/{id}")]
        [ProducesResponseType((200), Type = typeof(LivrosDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Desativar(long id)
        {
            try
            {
                var pessoa = _livroBussines.Desativar(id);

                return Ok(pessoa);
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPatch("ativar/{id}")]
        [ProducesResponseType((200), Type = typeof(LivrosDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Ativar(long id)
        {
            try
            {
                var pessoa = _livroBussines.Ativar(id);

                return Ok(pessoa);
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("autor/{autor}")]
        [ProducesResponseType((200), Type = typeof(List<LivrosDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetLivrosPorAutor(string autor)
        {
            try
            {
                var livros = _livroBussines.ObterLivrosPorAutor(autor);

                return Ok(livros);
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("titulo/{titulo}")]
        [ProducesResponseType((200), Type = typeof(List<LivrosDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetLivrosPorTitulo(string titulo)
        {
            try
            {
                var livros = _livroBussines.ObterLivrosPorTitulo(titulo);

                return Ok(livros);
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("dataLancamento/{dataLancamento}")]
        [ProducesResponseType((200), Type = typeof(List<LivrosDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetLivrosPorDataLancamento(DateTime dataLancamento)
        {
            try
            {
                var livros = _livroBussines.ObterLivrosPorDataLancamento(dataLancamento);

                return Ok(livros);
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("preco/{preco}")]
        [ProducesResponseType((200), Type = typeof(List<LivrosDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetLivrosPorPreco(decimal preco)
        {
            try
            {
                var livros = _livroBussines.ObterLivrosPorPreco(preco);

                return Ok(livros);
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("{direcaoOrdenacao}/{tamanhoPagina}/{paginaAtual}")]
        [ProducesResponseType((200), Type = typeof(List<LivrosDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get( string direcaoOrdenacao, int tamanhoPagina, int paginaAtual)
        {
            try
            {
                return Ok(_livroBussines.pagedSearchDbo(direcaoOrdenacao, tamanhoPagina, paginaAtual));
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPost()]
        [ProducesResponseType((200), Type = typeof(LivrosDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] LivrosDbo livro)
        {
            try
            {
                var criarLivros = _livroBussines.Criar(livro);

                return Ok(criarLivros);
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpPut]
        [ProducesResponseType((200), Type = typeof(LivrosDbo))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] LivrosDbo livro)
        {
            try
            {
                var atualizarLivro = _livroBussines.Atualizar(livro);

                return Ok(atualizarLivro);
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
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
                _livroBussines.Deletar(id);

                return NoContent();
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
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }
    }
}