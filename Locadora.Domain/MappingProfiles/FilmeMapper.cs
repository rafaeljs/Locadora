using AutoMapper;
using Locadora.Domain.Dtos;
using Locadora.Domain.Entities;

namespace Locadora.Domain.MappingProfiles
{
    public class FilmeMapper : Profile
    {
        public FilmeMapper()
        {
            CreateMap<FilmeDto, Filme>()
                .ForMember(c => c.Id, o => o.Ignore());

            CreateMap<Filme, FilmeDto>();
        }
    }
}
