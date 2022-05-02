using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Domain.Dtos
{
    public record LocacaoDto(int clienteId, int filmeId, DateTime dataLocacao);
}
