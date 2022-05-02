using Locadora.Domain.Entities;

namespace Locadora.Domain.Interfaces.Repositories
{
    public interface ILocacaoRepository : IRepositoryBase<Locacao>
    {
        bool FilmeJaAlocado(int filmeId);
    }
}
