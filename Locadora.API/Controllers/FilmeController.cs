using Locadora.Domain.Dtos;
using Locadora.Domain.Infra;
using Locadora.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmeController : ControllerBase
    {
        [HttpGet]
        public IActionResult ObterTodosFilmes([FromServices] IFilmeService service)
        {
            try
            {
                var lista = service.ObterTodosFilmes();

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<List<FilmeDto>>.Success(lista));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }

        [HttpPost]
        public IActionResult CadastrarFilme([FromServices] IFilmeService service, [FromBody] FilmeDto dto)
        {
            try
            {
                service.CadastrarFilme(dto);

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<string>.Success("Filme criado com sucesso"));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditarFilme([FromServices] IFilmeService service, [FromBody] FilmeDto dto, int id)
        {
            try
            {
                service.EditarFilme(id, dto);

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<string>.Success("Filme editado com sucesso"));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirFilme([FromServices] IFilmeService service, int id)
        {
            try
            {
                service.ExcluirFilme(id);

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<string>.Success("Filme excluido com sucesso"));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }
        [HttpPost("ImportarFilmesCSV")]
        public IActionResult ImportarFilmes([FromServices] IFilmeService service, [FromForm] CsvFile arquivo)
        {
            try
            {
                if(arquivo.File.ContentType != "application/vnd.ms-excel")
                    return BadRequest(ApiResponse<string>.Fail("Tipo de Arquivo não suportado"));

                service.ImportarFilmesCsv(arquivo.File);

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<string>.Success("Filmes importados com sucesso"));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }
    }
}
