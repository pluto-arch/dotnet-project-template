namespace PlutoNetCoreTemplate.Application.AppServices.ProductAppServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Aggregates.ProductAggregate;
    using Microsoft.EntityFrameworkCore;
    using Models.ProductModels;

    public class ProductAppService:IProductAppService
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductAppService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        public async Task<List<ProductModels>> GetListAsync()
        {
            var entities = await _productRepository.Include(x=>x.Devices).ToListAsync();
            var res = _mapper.Map<List<Product>, List<ProductModels>>(entities);
            return res;
        }

        public async Task<ProductModels> GetAsync(string key)
        {
            var entity = await _productRepository.Include(x=>x.Devices).FirstOrDefaultAsync(x => x.Id == key);
            return _mapper.Map<Product, ProductModels>(entity);
        }
    }
}