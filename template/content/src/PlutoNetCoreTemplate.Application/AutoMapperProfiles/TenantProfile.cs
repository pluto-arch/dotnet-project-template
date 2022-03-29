using PlutoNetCoreTemplate.Application.Models.TenantModels;

namespace PlutoNetCoreTemplate.Application.AutoMapperProfiles
{
    using AutoMapper;

    using Domain.Aggregates.TenantAggregate;

    public class TenantProfile : Profile
    {
        public TenantProfile()
        {

            CreateMap<Tenant, TenantDto>()
                .ForMember(x => x.Id, o => o.MapFrom(z => z.Id))
                .ForMember(x => x.Name, o => o.MapFrom(z => z.Name));
        }
    }
}