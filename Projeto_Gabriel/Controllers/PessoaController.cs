//using Asp.Versioning;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Projeto_Gabriel.Application.Dto;
//using Projeto_Gabriel.Application.Exceptions;
//using Projeto_Gabriel.Application.Hypermedia.Filters;
//using Projeto_Gabriel.Application.Utils;
//using Projeto_Gabriel.Bussines;
//using Swashbuckle.AspNetCore.Filters;

//namespace Projeto_Gabriel.Controllers
//{
//    [ApiVersion("1")]
//    [ApiController]
//    [Authorize("Bearer")]
//    [Route("v{version:apiVersion}/api/[controller]")]
//    public class PessoaController : ControllerBase
//    {
//        private readonly ILogger<PessoaController> _logger;
//        private IPessoaBussines _pessoaService;

//        public PessoaController(ILogger<PessoaController> logger, IPessoaBussines pessoaService)
//        {
//            _logger = logger;
//            _pessoaService = pessoaService;
//        }

//        [HttpGet("{direcaoOrdenacao}/{tamanhoPagina}/{paginaAtual}")]
//        [ProducesResponseType((200), Type = typeof(List<PessoaDbo>))]
//        [ProducesResponseType(typeof(ErrorResponse), 400)]
//        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 401)]
//        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 404)]
//        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 500)]
//        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
//        [TypeFilter(typeof(HyperMediaFilter))]
//        public IActionResult Get(string direcaoOrdenacao, int tamanhoPagina, int paginaAtual)
//        {
//            try
//            {
//                return Ok(_pessoaService.ObterComPaginacao(direcaoOrdenacao, tamanhoPagina, paginaAtual));
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(new { message = ex.Message });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
//            }
//        }

//        /// <summary>
//        /// Obtém os Pessoa por Id. (Endpoint liberado, não requer autenticação)
//        /// </summary>
//        [HttpGet("{id}")]
//        [AllowAnonymous]
//        [ProducesResponseType((200), Type = typeof(PessoaDbo))]
//        [ProducesResponseType(typeof(ErrorResponse), 400)]
//        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 401)]
//        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 404)]
//        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 500)]
//        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
//        [TypeFilter(typeof(HyperMediaFilter))]
//        public IActionResult Get(long id)
//        {
//            try
//            {
//                var pessoa = _pessoaService.ObterPorId(id);

//                return Ok(pessoa);
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(new { message = ex.Message });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
//            }
//        }

//        [HttpPatch("desativar/{id}")]
//        [ProducesResponseType((200), Type = typeof(PessoaDbo))]
//        [ProducesResponseType(typeof(ErrorResponse), 400)]
//        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 401)]
//        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 404)]
//        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 500)]
//        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
//        [TypeFilter(typeof(HyperMediaFilter))]
//        public IActionResult Desativar(long id)
//        {
//            try
//            {
//                var pessoa = _pessoaService.Desativar(id);

//                if (pessoa == null)
//                    return NotFound();

//                return Ok(pessoa);
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(new { message = ex.Message });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
//            }
//        }

//        [HttpPatch("ativar/{id}")]
//        [ProducesResponseType((200), Type = typeof(PessoaDbo))]
//        [ProducesResponseType(typeof(ErrorResponse), 400)]
//        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 401)]
//        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 404)]
//        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 500)]
//        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
//        [TypeFilter(typeof(HyperMediaFilter))]
//        public IActionResult Ativar(long id)
//        {
//            try
//            {
//                var pessoa = _pessoaService.Ativar(id);

//                if (pessoa == null)
//                    return NotFound();

//                return Ok(pessoa);
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(new { message = ex.Message });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = "Erro interno no servidor", details = ex.Message });
//            }
//        }

//        [HttpGet("nome/{nome}")]
//        [ProducesResponseType((200), Type = typeof(List<PessoaDbo>))]
//        [ProducesResponseType(typeof(ErrorResponse), 400)]
//        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 401)]
//        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 404)]
//        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 500)]
//        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
//        [TypeFilter(typeof(HyperMediaFilter))]
//        public IActionResult ObterPessoasPorNome(string nome)
//        {
//            try
//            {
//                var pessoa = _pessoaService.ObterPessoasPorNome(nome);
//                if (pessoa == null)
//                    return NotFound();
//                return Ok(pessoa);
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(new { message = ex.Message });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
//            }
//        }

//        [HttpGet("endereco/{endereco}")]
//        [ProducesResponseType((200), Type = typeof(List<PessoaDbo>))]
//        [ProducesResponseType(typeof(ErrorResponse), 400)]
//        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 401)]
//        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 404)]
//        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 500)]
//        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
//        [TypeFilter(typeof(HyperMediaFilter))]
//        public IActionResult ObterPessoasPorEndereco(string endereco)
//        {
//            try
//            {
//                var pessoa = _pessoaService.ObterPessoasPorEndereco(endereco);

//                if (pessoa == null)
//                    return NotFound();

//                return Ok(pessoa);
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(new { message = ex.Message });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
//            }
//        }

//        [HttpPost()]
//        [ProducesResponseType((200), Type = typeof(PessoaDbo))]
//        [ProducesResponseType(typeof(ErrorResponse), 400)]
//        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 401)]
//        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 404)]
//        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 500)]
//        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
//        [TypeFilter(typeof(HyperMediaFilter))]
//        public IActionResult Post([FromBody] PessoaDbo pessoa)
//        {
//            try
//            {
//                if (pessoa == null)
//                    return BadRequest();

//                return Ok(_pessoaService.Criar(pessoa));
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(new { message = ex.Message });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
//            }
//        }

//        [HttpPut]
//        [ProducesResponseType((200), Type = typeof(PessoaDbo))]
//        [ProducesResponseType(typeof(ErrorResponse), 400)]
//        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 401)]
//        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 404)]
//        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 500)]
//        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
//        [TypeFilter(typeof(HyperMediaFilter))]
//        public IActionResult Put([FromBody] PessoaDbo pessoa)
//        {
//            try
//            {
//                if (pessoa == null)
//                    return BadRequest();

//                return Ok(_pessoaService.Atualizar(pessoa));
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
//            }
//        }

//        [HttpDelete("{id}")]
//        [ProducesResponseType(typeof(ErrorResponse), 400)]
//        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 401)]
//        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 404)]
//        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
//        [ProducesResponseType(typeof(ErrorResponse), 500)]
//        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
//        [TypeFilter(typeof(HyperMediaFilter))]
//        public IActionResult Delete(long id)
//        {
//            try
//            {
//                _pessoaService.Deletar(id);
//                return NoContent();
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
//            }
//        }
//    }
//}