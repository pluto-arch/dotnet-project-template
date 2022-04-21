namespace PlutoNetCoreTemplate.Application.AutoMapperProfiles
{
    using Domain.Aggregates.ProductAggregate;
    using Models.ProductModels;

    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductGetResponseDto>()
                .ForMember(x => x.Id, o => o.MapFrom(z => z.Id))
                .ForMember(x => x.Name, o => o.MapFrom(z => z.Name))
                .ForMember(x => x.Remark, o => o.MapFrom(z => z.Remark));


            CreateMap<ProductCreateOrUpdateRequestDto, Product>();


            CreateMap<Device, DeviceGetResponseDto>()
                .ForMember(x => x.Id, o => o.MapFrom(z => z.Id))
                .ForMember(x => x.SerialNo, o => o.MapFrom(z => z.SerialNo))
                .ForMember(x => x.Coordinate, o => o.MapFrom(z => z.Coordinate.ToString()))
                .ForMember(x => x.Online, o => o.MapFrom(z => z.Online));
        }
    }
}