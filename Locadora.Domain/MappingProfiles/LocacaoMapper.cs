using AutoMapper;
using Locadora.Domain.Dtos;
using Locadora.Domain.Entities;

namespace Locadora.Domain.MappingProfiles
{
    public class LocacaoMapper : Profile
    {
        public LocacaoMapper()
        {
            CreateMap<LocacaoDto, Locacao>()
                .ForMember(l => l.DataLocacao, o => o.MapFrom(l => l.dataLocacao.Date))
                .ReverseMap();
        }
    }
}
