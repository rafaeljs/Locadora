using AutoMapper;
using Locadora.Domain.Dtos;
using Locadora.Domain.Entities;

namespace Locadora.Domain.MappingProfiles
{
    public class ClienteMapper : Profile
    {
        public ClienteMapper()
        {
            CreateMap<ClienteDto, Cliente>()
                .ForMember(c => c.Id, o => o.Ignore());

            CreateMap<Cliente,ClienteDto>();
        }
    }
}
