using Locadora.Domain.Entities;
using Locadora.Domain.Interfaces.Repositories;
using Locadora.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Repository.Repositories
{
    public class FilmeRepository : RepositoryBase<Filme>, IFilmeRepository
    {
         public FilmeRepository(LocadoraContext context) : base(context) { }

    }
}
