using AutoMapper;
using ClosedXML.Excel;
using Locadora.Domain.Dtos;
using Locadora.Domain.Entities;
using Locadora.Domain.Infra;
using Locadora.Domain.Interfaces.Repositories;
using Locadora.Domain.Interfaces.Services;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Locadora.Service
{
    public class LocacaoService : Notify, ILocacaoService
    {
        private readonly ILocacaoRepository _locacaoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IFilmeRepository _filmeRepository;
        private readonly IMapper _mapper;
        public LocacaoService(ILocacaoRepository locacaoRepository, IClienteRepository clienteRepository,
            IFilmeRepository filmeRepository, IMapper mapper)
        {
            _locacaoRepository = locacaoRepository;
            _clienteRepository = clienteRepository;
            _filmeRepository = filmeRepository;
            _mapper = mapper;
        }

        public void ALocarFilme(LocacaoDto dto)
        {
            Validate(() => !_clienteRepository.Exist(c => c.Id == dto.clienteId), "Cliente não encontrado!");
            Validate(() => !_filmeRepository.Exist(c => c.Id == dto.filmeId), "Filme não encontrado!");
            Validate(() => _locacaoRepository.FilmeJaAlocado(dto.filmeId), "Filme ja alocado!", false);

            if (!IsValid) return;

            Locacao locacao = _mapper.Map<Locacao>(dto);

            _locacaoRepository.Add(locacao);
            _locacaoRepository.Save();
        }

        public void DevolverFilme(int id, DateTime dataDevolucao)
        {
            Validate(() => !_locacaoRepository.Exist(c => c.Id == id), "Locação não encontrado!");

            if (!IsValid) return;

            Locacao locacao = _locacaoRepository.GetById(id);
            Validate(() => locacao.DataLocacao > dataDevolucao.Date, "Data de devolução não pode ser anterior a data de locação!");

            if (!IsValid) return;

            locacao.AlterarDataDevolucao(dataDevolucao.Date);

            _locacaoRepository.Update(locacao);
            _locacaoRepository.Save();
        }

        public byte[] ObterRelatorioDeClientesEmAtraso()
        {
            var dataHoje = DateTime.Now.Date;
            var clientesEmAtraso = _locacaoRepository.GetByPredicate(x => x.DataDevolucao == null)
                .ToList()
                .Where(x => x.Filme.Lancamento ? 
                    (dataHoje - x.DataLocacao).TotalDays > 2 : 
                    (dataHoje - x.DataLocacao).TotalDays > 3)
            .Select(x => x.Cliente).Distinct().ToList();

            Validate(() => !clientesEmAtraso.Any(), "Não há clientes em atraso!");

            if (!IsValid) return null;

            return GerarExcelClientesEmAtraso(clientesEmAtraso);
        }

        private byte[] GerarExcelClientesEmAtraso(List<Cliente> clientes)
        {
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                workbook.Worksheets.Add("Clientes em atraso");
                worksheet.Cell(1, 1).Value = "Clientes que possuem algum filme em atraso para devolução";
                worksheet.Cell(2, 1).Value = "Id";
                worksheet.Cell(2, 2).Value = "Nome";
                worksheet.Cell(2, 3).Value = "CPF";

                int linha = 3;
                foreach (var cliente in clientes)
                {
                    worksheet.Cell(linha, 1).Value = cliente.Id;
                    worksheet.Cell(linha, 2).Value = cliente.Nome;
                    worksheet.Cell(linha, 3).Value = cliente.CPF;
                    worksheet.Cell(linha, 3).DataType = XLDataType.Text;

                    linha++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public byte[] ObterRelatorioDeFilmesNuncaAlugados()
        {
            var filmesNuncaAlugados = _filmeRepository.GetByPredicate(x => !x.Locacoes.Any())
                .ToList();

            Validate(() => !filmesNuncaAlugados.Any(), "Todos filmes já foram alugados ao menos uma vez!");

            if (!IsValid) return null;

            return GerarExcelFilmesNuncaAlugados(filmesNuncaAlugados);
        }

        private byte[] GerarExcelFilmesNuncaAlugados(List<Filme> filmes)
        {
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                workbook.Worksheets.Add("Filmes nunca alugados");
                worksheet.Cell(1, 1).Value = "Filmes que nunca foram alugados";
                worksheet.Cell(2, 1).Value = "Id";
                worksheet.Cell(2, 2).Value = "Titulo";
                worksheet.Cell(2, 3).Value = "Classificação Indicativa";
                worksheet.Cell(2, 4).Value = "Lançamento";

                int linha = 3;
                foreach (var filme in filmes)
                {
                    worksheet.Cell(linha, 1).Value = filme.Id;
                    worksheet.Cell(linha, 2).Value = filme.Titulo;
                    worksheet.Cell(linha, 3).Value = filme.ClassificacaoIndicativa;
                    worksheet.Cell(linha, 4).Value = filme.Lancamento ? "Sim" : "Não";

                    linha++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public byte[] ObterRelatorioTop5FilmesAlugadosDoUltimoAno()
        {
            var ultimoAno = DateTime.Now.Date.AddYears(-1);
            var filmesUltimoAno = _locacaoRepository.GetByPredicate(x => x.DataLocacao > ultimoAno)
                .ToList();

            var top5UltimoAno = filmesUltimoAno
                .GroupBy(x => x.Filme)
                .Select(x => new TopFilmesDto(x.Key, x.Count()))
                .OrderByDescending(x => x.Quantidade)
                .Take(5)
                .ToList();

            Validate(() => !top5UltimoAno.Any(), "Não há filmes alugado no ultimo ano!");

            if (!IsValid) return null;

            return GerarExcelTop5FilmesAlugadosDoUltimoAno(top5UltimoAno);
        }

        private byte[] GerarExcelTop5FilmesAlugadosDoUltimoAno(List<TopFilmesDto> filmes)
        {
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                workbook.Worksheets.Add("Top 5 filmes");
                worksheet.Cell(1, 1).Value = "Top 5 filmes mais alugados no ultimo ano";
                worksheet.Cell(2, 1).Value = "Id";
                worksheet.Cell(2, 2).Value = "Titulo";
                worksheet.Cell(2, 3).Value = "Classificação Indicativa";
                worksheet.Cell(2, 4).Value = "Lançamento";
                worksheet.Cell(2, 5).Value = "Quantidade";

                int linha = 3;
                foreach (var top in filmes)
                {
                    worksheet.Cell(linha, 1).Value = top.Filme.Id;
                    worksheet.Cell(linha, 2).Value = top.Filme.Titulo;
                    worksheet.Cell(linha, 3).Value = top.Filme.ClassificacaoIndicativa;
                    worksheet.Cell(linha, 4).Value = top.Filme.Lancamento ? "Sim" : "Não";
                    worksheet.Cell(linha, 5).Value = top.Quantidade;

                    linha++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public byte[] ObterRelatorioTop3FilmesMenosAlugadosDaUltimaSemana()
        {
            var ultimaSemana = DateTime.Now.Date.AddDays(-7);
            var filmes = _filmeRepository.GetAll()
                .ToList();

            var Top3FilmesMenosAlugados = filmes
                .OrderBy(x => x.Locacoes.Count(z => z.DataLocacao > ultimaSemana))
                .Take(3)
                .ToList();

            Validate(() => !Top3FilmesMenosAlugados.Any(), "Não há filmes alugado no ultima Semana!");

            if (!IsValid) return null;

            return GerarExcelTop3FilmesMenosAlugadosDoUltimaSemana(Top3FilmesMenosAlugados, ultimaSemana);
        }

        private byte[] GerarExcelTop3FilmesMenosAlugadosDoUltimaSemana(List<Filme> filmes, DateTime ultimaSemana)
        {
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                workbook.Worksheets.Add("Top 3 filmes");
                worksheet.Cell(1, 1).Value = "Top 3 filmes menos alugados no ultimo ano";
                worksheet.Cell(2, 1).Value = "Id";
                worksheet.Cell(2, 2).Value = "Titulo";
                worksheet.Cell(2, 3).Value = "Classificação Indicativa";
                worksheet.Cell(2, 4).Value = "Lançamento";
                worksheet.Cell(2, 5).Value = "Quantidade";

                int linha = 3;
                foreach (var filme in filmes)
                {
                    worksheet.Cell(linha, 1).Value = filme.Id;
                    worksheet.Cell(linha, 2).Value = filme.Titulo;
                    worksheet.Cell(linha, 3).Value = filme.ClassificacaoIndicativa;
                    worksheet.Cell(linha, 4).Value = filme.Lancamento ? "Sim" : "Não";
                    worksheet.Cell(linha, 5).Value = filme.Locacoes.Count(z => z.DataLocacao > ultimaSemana);

                    linha++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public byte[] ObterRelatorioSegundoClienteMaisAlugouFilmes()
        {
            var ultimaSemana = DateTime.Now.Date.AddDays(-7);
            var clientes = _clienteRepository
                .GetAll()
                .OrderByDescending(x => x.Locacoes.Count())
                .Take(2)
                .ToList();

            Validate(() => clientes.Count() < 2, "Não há 2 clientes para o relatório!");

            if (!IsValid) return null;

            return GerarExcelSegundoClienteMaisAlugouFilmes(clientes.Last());
        }

        private byte[] GerarExcelSegundoClienteMaisAlugouFilmes(Cliente cliente)
        {
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                workbook.Worksheets.Add("Segundo Cliente");
                worksheet.Cell(1, 1).Value = "Segundo Cliente que mais alugou filmes";
                worksheet.Cell(2, 1).Value = "Cliente";

                worksheet.Cell(3, 1).Value = "Id";
                worksheet.Cell(3, 2).Value = "Nome";
                worksheet.Cell(3, 3).Value = "CPF";

                worksheet.Cell(4, 1).Value = cliente.Id;
                worksheet.Cell(4, 2).Value = cliente.Nome;
                worksheet.Cell(4, 3).Value = cliente.CPF;
                worksheet.Cell(4, 3).DataType = XLDataType.Text;

                worksheet.Cell(5, 1).Value = "Titulo";
                worksheet.Cell(5, 2).Value = "Classificação Indicativa";
                worksheet.Cell(5, 3).Value = "Lançamento";
                worksheet.Cell(5, 4).Value = "Data de Locação";
                worksheet.Cell(5, 5).Value = "Data de Devolução";

                int linha = 6;
                if (!cliente.Locacoes.Any())
                {
                    worksheet.Cell(linha, 1).Value = "Cliente não Alugou Nenhum Filme";
                }
                foreach (var locacao in cliente.Locacoes)
                {
                    worksheet.Cell(linha, 1).Value = locacao.Filme.Titulo;
                    worksheet.Cell(linha, 2).Value = locacao.Filme.ClassificacaoIndicativa;
                    worksheet.Cell(linha, 3).Value = locacao.Filme.Lancamento;
                    worksheet.Cell(linha, 4).Value = locacao.DataLocacao.ToString("dd/MM/yyyy");
                    worksheet.Cell(linha, 5).Value = locacao.DataDevolucao.HasValue ? locacao.DataDevolucao.Value.ToString("dd/MM/yyyy") : "Não devolveu ainda.";

                    linha++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
