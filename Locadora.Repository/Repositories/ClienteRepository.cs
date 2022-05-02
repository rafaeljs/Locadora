using Locadora.Domain.Entities;
using Locadora.Domain.Interfaces.Repositories;
using Locadora.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Repository.Repositories
{
    public class ClienteRepository : RepositoryBase<Cliente>, IClienteRepository
    {
         public ClienteRepository(LocadoraContext context) : base(context) { }
    }
}
