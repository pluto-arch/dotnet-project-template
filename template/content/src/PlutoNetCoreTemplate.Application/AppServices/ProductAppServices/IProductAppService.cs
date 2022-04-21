namespace PlutoNetCoreTemplate.Application.AppServices.ProductAppServices
{
    using Generics;
    using Models.ProductModels;

    public interface IProductAppService
        : ICrudAppService<string, ProductGetResponseDto, ProductPagedRequestDto, ProductGetResponseDto, ProductCreateOrUpdateRequestDto, ProductCreateOrUpdateRequestDto>
    {
        Task<ProductGetResponseDto> GetByName(string productName);
    }
}