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
    public class LocacaoRepository : RepositoryBase<Locacao>, ILocacaoRepository
    {
        public LocacaoRepository(LocadoraContext context) : base(context) { }

        public bool FilmeJaAlocado(int filmeId)
        {
            return this.Exist(f => f.FilmeId == filmeId && f.DataDevolucao == null);
        }
    }
}
