using Locadora.Domain.Dtos;
using Locadora.Domain.Entities;
using Locadora.Domain.Interfaces.Infra;

namespace Locadora.Domain.Interfaces.Services
{
    public interface ILocacaoService : INotify
    {
        void ALocarFilme(LocacaoDto dto);
        void DevolverFilme(int id, DateTime dataDevolucao);
        byte[] ObterRelatorioDeClientesEmAtraso();
        byte[] ObterRelatorioDeFilmesNuncaAlugados();
        byte[] ObterRelatorioTop5FilmesAlugadosDoUltimoAno();
        byte[] ObterRelatorioTop3FilmesMenosAlugadosDaUltimaSemana();
        byte[] ObterRelatorioSegundoClienteMaisAlugouFilmes();
    }
}
