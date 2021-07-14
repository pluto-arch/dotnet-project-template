using AutoMapper;


namespace PlutoNetCoreTemplate.Application
{
    using Domain.Aggregates.ProductAggregate;
    using Domain.Aggregates.TenantAggregate;

    using Models.ProductModels;
    using Models.TenantModels;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductModels>()
                .ForMember(x => x.Id, o => o.MapFrom(z => z.Id))
                .ForMember(x => x.Name, o => o.MapFrom(z => z.Name))
                .ForMember(x => x.Devices, o => o.MapFrom(z => z.Devices))
                .ForMember(x => x.Remark, o => o.MapFrom(z => z.Remark));


            CreateMap<Device, DeviceModel>()
                .ForMember(x => x.Id, o => o.MapFrom(z => z.Id))
                .ForMember(x => x.SerialNo, o => o.MapFrom(z => z.SerialNo))
                .ForMember(x => x.Coordinate, o => o.MapFrom(z => z.Coordinate.ToString()))
                .ForMember(x => x.Online, o => o.MapFrom(z => z.Online));


            CreateMap<Tenant, TenantModel>()
                .ForMember(x => x.Id, o => o.MapFrom(z => z.Id))
                .ForMember(x => x.Name, o => o.MapFrom(z => z.Name));
        }
    }
}