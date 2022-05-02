using Locadora.Domain.Dtos;
using Locadora.Domain.Interfaces.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Domain.Interfaces.Services
{
    public interface IClienteService : INotify
    {
        void CadastrarCliente(ClienteDto dto);
        void EditarCliente(int id, ClienteDto dto);
        void ExcluirCliente(int id);
        List<ClienteDto> ObterTodosClientes();
    }
}
