using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Locadora.Domain.Dtos;
using Locadora.Domain.Entities;
using Locadora.Domain.Helpers;
using Locadora.Domain.Infra;
using Locadora.Domain.Interfaces.Repositories;
using Locadora.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text;

namespace Locadora.Service
{
    public class FilmeService : Notify, IFilmeService
    {
        private readonly IFilmeRepository _filmeRepository;
        private readonly IMapper _mapper;

        public FilmeService(IFilmeRepository filmeRepository, IMapper mapper)
        {
            _filmeRepository = filmeRepository;
            _mapper = mapper;
        }

        public List<FilmeDto> ObterTodosFilmes()
        {
            var lista = _filmeRepository.GetAll().ToList();
            return _mapper.Map<List<FilmeDto>>(lista);
        }

        public void CadastrarFilme(FilmeDto dto)
        {
            var (_, titulo, classificacao, _) = dto;
            ValidarFilme(titulo, classificacao, true); 

            if (!IsValid) return;

            Filme filme = _mapper.Map<Filme>(dto);
            _filmeRepository.Add(filme);
            _filmeRepository.Save();
        }

        public void EditarFilme(int id, FilmeDto dto)
        {
            var (_, titulo, classificacao, _) = dto;
            ValidarFilme(titulo, classificacao);
            Validate(() => !_filmeRepository.Exist(c => c.Id == id), "Filme não encontrado!");

            if (!IsValid) return;

            Filme filme = _filmeRepository.GetById(id);
            filme.AlterarTitulo(dto.titulo);
            filme.AlterarClassificacao(dto.classificacaoIndicativa);
            filme.AlterarLancamento(dto.lancamento);

            _filmeRepository.Update(filme);
            _filmeRepository.Save();
        }

        public void ExcluirFilme(int id)
        {
            Validate(() => !_filmeRepository.Exist(c => c.Id == id), "Filme não encontrado!");
            if (!IsValid) return;

            Validate(() => _filmeRepository.Exist(c => c.Id == id && c.Locacoes.Any()), "Filme já foi alugado, não é possivel remover o registro!");
            if (!IsValid) return;

            Filme filme = _filmeRepository.GetById(id);

            _filmeRepository.Remove(filme);
            _filmeRepository.Save();
        }

        public void ImportarFilmesCsv(IFormFile arquivo)
        {
            var filmes = new List<Filme>();
            try
            {
                using StreamReader reader = new StreamReader(arquivo.OpenReadStream(), Encoding.UTF8);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture) { 
                    Delimiter = ";", 
                    Encoding = Encoding.UTF8,
                    PrepareHeaderForMatch = args => args.Header.ToLower()
                };

                using (var csvReader = new CsvReader(reader, config))
                {
                    filmes = csvReader.GetRecords<Filme>().ToList();
                }

                _filmeRepository.BeginTransaction();
                foreach (var filme in filmes)
                {
                    ValidarFilme(filme.Titulo, filme.ClassificacaoIndicativa, true);
                    Validate(() => _filmeRepository.Exist(c => c.Id == filme.Id), $"Id {filme.Id} já cadastrado!");

                    if (!IsValid)
                    {
                        _filmeRepository.RollbackTransaction();
                        return;
                    };

                    _filmeRepository.Add(filme);
                    _filmeRepository.Save();
                }

                _filmeRepository.CommitTransaction();
            }
            catch (CsvHelperException ex)
            {
                // catch exception
            }
        }

        private void ValidarFilme(string titulo, int classificacaoIndicativa, bool novo = false)
        {
            Validate(() => string.IsNullOrEmpty(titulo), "Titulo não pode ser vazio!");
            Validate(() => classificacaoIndicativa <= 0, "Classificação Indicativa não pode ser menor que 1!");

            if (novo)
                Validate(() => _filmeRepository.Exist(f => f.Titulo.Equals(titulo)), "Já existe um filme com o titulo informado!", false);
        }
    }
}
