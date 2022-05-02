using Locadora.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Locadora.Domain.Dtos
{
    public record FilmeDto(int id, string titulo, int classificacaoIndicativa, bool lancamento);
    public record TopFilmesDto(Filme Filme, int Quantidade);
    public record CsvFile(IFormFile File);
}
