using AutoMapper;
using Locadora.Domain.Dtos;
using Locadora.Domain.Entities;
using Locadora.Domain.Helpers;
using Locadora.Domain.Infra;
using Locadora.Domain.Interfaces.Repositories;
using Locadora.Domain.Interfaces.Services;

namespace Locadora.Service
{
    public class ClienteService : Notify, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public List<ClienteDto> ObterTodosClientes()
        {
            var lista = _clienteRepository.GetAll().ToList();
            return _mapper.Map<List<ClienteDto>>(lista);
        }

        public void CadastrarCliente(ClienteDto dto)
        {
            ValidarDto(dto, true);

            if (!IsValid) return;

            Cliente cliente = _mapper.Map<Cliente>(dto);
            _clienteRepository.Add(cliente);
            _clienteRepository.Save();
        }

        public void EditarCliente(int id, ClienteDto dto)
        {
            ValidarDto(dto, false);
            Validate(() => !_clienteRepository.Exist(c => c.Id == id), "Cliente não encontrado!");

            if (!IsValid) return;

            Cliente cliente = _clienteRepository.GetById(id);
            cliente.AlterarNome(dto.nome);
            cliente.AlterarCPF(dto.cpf);
            cliente.AlterarDataNascimento(dto.dataNascimento);

            _clienteRepository.Update(cliente);
            _clienteRepository.Save();
        }

        public void ExcluirCliente(int id)
        {
            Validate(() => !_clienteRepository.Exist(c => c.Id == id), "Cliente não encontrado!");
            if (!IsValid) return;

            Validate(() => _clienteRepository.Exist(c => c.Id == id && c.Locacoes.Any()), "Cliente já alugou algum filme, não é possivel remover o registro!");
            if (!IsValid) return;

            Cliente cliente = _clienteRepository.GetById(id);

            _clienteRepository.Remove(cliente);
            _clienteRepository.Save();
        }

        private void ValidarDto(ClienteDto dto, bool novo = false)
        {
            var (_, nome, cpf, dataNascimento) = dto;

            Validate(() => string.IsNullOrEmpty(nome), "Nome completo não pode ser vazio!");
            Validate(() => string.IsNullOrEmpty(cpf), "CPF não pode ser vazio!");
            Validate(() => dataNascimento == DateTime.MinValue || dataNascimento == DateTime.MaxValue, "Data de Nascimento inválida!");
            Validate(() => !cpf.ValidarCPF(), "CPF inválido!", false);
            Validate(() => nome.Length > 200, "Nome não pode ter mais de 200 caracteres!", false);

            if (novo)
                Validate(() => _clienteRepository.Exist(u => u.CPF.Equals(cpf)), "CPF já cadastrado", false);
        }
    }
}
