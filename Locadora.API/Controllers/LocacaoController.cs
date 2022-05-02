using Locadora.Domain.Dtos;
using Locadora.Domain.Infra;
using Locadora.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocacaoController : ControllerBase
    {
        [HttpPost]
        public IActionResult AlocarFilme([FromServices] ILocacaoService service, [FromBody] LocacaoDto dto)
        {
            try
            {
                service.ALocarFilme(dto);

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<string>.Success("Filme alocado com sucesso"));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }

        [HttpPut("{id}")]
        public IActionResult DevolverFilme([FromServices] ILocacaoService service, [FromBody] DateTime dataDevolucao, int id)
        {
            try
            {
                service.DevolverFilme(id, dataDevolucao);

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<string>.Success("Filme devolvido com sucesso"));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }

        [HttpGet("ObterRelatorioDeClientesEmAtraso")]
        public IActionResult ObterRelatorioDeClientesEmAtraso([FromServices] ILocacaoService service)
        {
            try
            {
                var relatorio = service.ObterRelatorioDeClientesEmAtraso();

                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Clientes_Em_Atraso.xlsx";

                if (!service.IsValid)
                    return BadRequest(service.GetConcatErrors());

                return File(relatorio, contentType, fileName);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ObterRelatorioDeFilmesNuncaAlugados")]
        public IActionResult ObterRelatorioDeFilmesNuncaAlugados([FromServices] ILocacaoService service)
        {
            try
            {
                var relatorio = service.ObterRelatorioDeFilmesNuncaAlugados();

                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Filmes_Nunca_Alugados.xlsx";

                if (!service.IsValid)
                    return BadRequest(service.GetConcatErrors());

                return File(relatorio, contentType, fileName);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ObterRelatorioTop5FilmesAlugadosDoUltimoAno")]
        public IActionResult ObterRelatorioTop5FilmesAlugadosDoUltimoAno([FromServices] ILocacaoService service)
        {
            try
            {
                var relatorio = service.ObterRelatorioTop5FilmesAlugadosDoUltimoAno();

                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Top5_Filmes_Alugados_Ultimo_Ano.xlsx";

                if (!service.IsValid)
                    return BadRequest(service.GetConcatErrors());

                return File(relatorio, contentType, fileName);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ObterRelatorioTop3FilmesMenosAlugadosDaUltimaSemana")]
        public IActionResult ObterRelatorioTop3FilmesMenosAlugadosDaUltimaSemana([FromServices] ILocacaoService service)
        {
            try
            {
                var relatorio = service.ObterRelatorioTop3FilmesMenosAlugadosDaUltimaSemana();

                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Top3_Filmes_Menos_Alugados_Ultima_Semana.xlsx";

                if (!service.IsValid)
                    return BadRequest(service.GetConcatErrors());

                return File(relatorio, contentType, fileName);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ObterRelatorioSegundoClienteMaisAlugouFilmes")]
        public IActionResult ObterRelatorioSegundoClienteMaisAlugouFilmes([FromServices] ILocacaoService service)
        {
            try
            {
                var relatorio = service.ObterRelatorioSegundoClienteMaisAlugouFilmes();

                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Segundo_Mais_Alugou_Filmes.xlsx";

                if (!service.IsValid)
                    return BadRequest(service.GetConcatErrors());

                return File(relatorio, contentType, fileName);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
