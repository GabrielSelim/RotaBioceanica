using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_Gabriel.Application.Dto;
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
    public class FileController : ControllerBase
    {
        private readonly IFileBusiness _fileBusiness;

        public FileController(IFileBusiness fileBusiness)
        {
            _fileBusiness = fileBusiness;
        }

        [HttpGet("{fileName}")]
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
        public IActionResult GetFile(string fileName)
        {
            try
            {
                var fileBytes = _fileBusiness.GetFile(fileName);
                var fileType = _fileBusiness.GetContentType(fileName);
                return File(fileBytes, fileType, fileName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost("uploadFiles")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(200, Type = typeof(List<FIleDetailDbo>))]
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
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            try
            {
                List<FIleDetailDbo> fileDetails = await _fileBusiness.SaveFilesToDisk(files);
                return Ok(fileDetails);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("listFiles")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(200, Type = typeof(List<FIleDetailDbo>))]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [SwaggerResponseExample(400, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [SwaggerResponseExample(401, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [SwaggerResponseExample(404, typeof(ErrorResponseExample))]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [SwaggerResponseExample(500, typeof(ErrorResponseExample))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult ListFiles()
        {
            try
            {
                var files = _fileBusiness.ListFiles();
                return Ok(files);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost("salvarImagemPokemon")]
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
        public async Task<IActionResult> SalvarImagemPokemon([FromForm] List<IFormFile> files)
         {
            try
            {
                var result = await _fileBusiness.SaveImagesToDatabase(files);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}