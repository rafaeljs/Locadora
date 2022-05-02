using Locadora.Domain.Dtos;
using Locadora.Domain.Infra;
using Locadora.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpGet]
        public IActionResult ObterTodosClientes([FromServices] IClienteService service)
        {
            try
            {
                var lista = service.ObterTodosClientes();

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<List<ClienteDto>>.Success(lista));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }

        [HttpPost]
        public IActionResult CadastrarCliente([FromServices] IClienteService service, [FromBody] ClienteDto dto)
        {
            try
            {
                service.CadastrarCliente(dto);

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<string>.Success("Cliente criado com sucesso"));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditarCliente([FromServices] IClienteService service, [FromBody] ClienteDto dto, int id)
        {
            try
            {
                service.EditarCliente(id, dto);

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<string>.Success("Cliente editado com sucesso"));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirCliente([FromServices] IClienteService service, int id)
        {
            try
            {
                service.ExcluirCliente(id);

                if (!service.IsValid)
                    return BadRequest(ApiResponse<string>.Fail(service.GetConcatErrors()));

                return Ok(ApiResponse<string>.Success("Cliente excluido com sucesso"));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Fail(e.Message));
            }
        }
    }
}
