using Locadora.Domain.Dtos;
using Locadora.Domain.Interfaces.Infra;
using Microsoft.AspNetCore.Http;

namespace Locadora.Domain.Interfaces.Services
{
    public interface IFilmeService : INotify
    {
        List<FilmeDto> ObterTodosFilmes();
        void CadastrarFilme(FilmeDto dto);
        void EditarFilme(int id, FilmeDto dto);
        void ExcluirFilme(int id);
        void ImportarFilmesCsv(IFormFile arquivo);
    }
}
