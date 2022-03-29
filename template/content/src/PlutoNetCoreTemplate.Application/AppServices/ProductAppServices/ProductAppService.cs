namespace PlutoNetCoreTemplate.Application.AppServices.ProductAppServices
{
    using AutoMapper;

    using Domain.Aggregates.ProductAggregate;
    using Domain.Repositories;

    using Generics;

    using Models.ProductModels;

    using System.Linq;
    using System.Threading.Tasks;

    public class ProductAppService
        : EntityKeyCrudAppService<Product, string, ProductGetResponseDto, ProductPagedRequestDto, ProductGetResponseDto, ProductCreateOrUpdateRequestDto, ProductCreateOrUpdateRequestDto>, IProductAppService
    {
        public ProductAppService(IRepository<Product, string> repository, IMapper mapper) : base(repository, mapper)
        {
        }


        protected override IQueryable<Product> CreateFilteredQuery(ProductPagedRequestDto requestModel)
        {
            if (requestModel.Keyword is not null && !string.IsNullOrWhiteSpace(requestModel.Keyword))
            {
                return Repository.Query.Where(e => e.Name.Contains(requestModel.Keyword));
            }

            return base.CreateFilteredQuery(requestModel);
        }

        public async Task<ProductGetResponseDto> GetByName(string productName)
        {
            var res = await Repository.GetAsync(p => p.Name == productName);
            return Mapper.Map<ProductGetResponseDto>(res);
        }
    }
}