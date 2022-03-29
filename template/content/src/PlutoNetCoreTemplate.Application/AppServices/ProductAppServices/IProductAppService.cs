namespace PlutoNetCoreTemplate.Application.AppServices.ProductAppServices
{
    using Generics;

    using Models.ProductModels;

    using System.Threading.Tasks;

    public interface IProductAppService
        : ICrudAppService<string, ProductGetResponseDto, ProductPagedRequestDto, ProductGetResponseDto, ProductCreateOrUpdateRequestDto, ProductCreateOrUpdateRequestDto>
    {
        Task<ProductGetResponseDto> GetByName(string productName);
    }
}