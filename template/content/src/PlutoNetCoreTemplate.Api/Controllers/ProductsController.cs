namespace PlutoNetCoreTemplate.Api.Controllers
{
    using Application.AppServices.ProductAppServices;
    using Application.Command;
    using Application.Models.Generics;
    using Application.Models.ProductModels;
    using Application.Permissions;


    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(ProductPermission.Product.Default)]
    public class ProductsController : BaseController<ProductsController>
    {

        private IProductAppService ProductAppService => LazyGetRequiredService<IProductAppService>();

        public ProductsController(ILazyLoadServiceProvider lazyLoad) : base(lazyLoad)
        {
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ServiceResponse<PageResponseDto<ProductGetResponseDto>>> Get([FromQuery] ProductPagedRequestDto request)
        {
            return ServiceResponse<PageResponseDto<ProductGetResponseDto>>.Success(await ProductAppService.GetListAsync(request));
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ServiceResponse<ProductGetResponseDto>> Get(string id)
        {
            return ServiceResponse<ProductGetResponseDto>.Success(await ProductAppService.GetAsync(id));
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        [Authorize(ProductPermission.Product.Create)]
        public async Task<ServiceResponse<ProductGetResponseDto>> Post([FromBody] ProductCreateOrUpdateRequestDto request)
        {
            return ServiceResponse<ProductGetResponseDto>.Success(await ProductAppService.CreateAsync(request));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="value"></param>
        [HttpPut]
        [Authorize(ProductPermission.Product.Edit)]
        public async Task<ServiceResponse<ProductGetResponseDto>> Put([FromBody] ProductCreateOrUpdateRequestDto value)
        {
            if (string.IsNullOrEmpty(value.Id))
            {
                return ServiceResponse<ProductGetResponseDto>.ValidateFailure();
            }
            return ServiceResponse<ProductGetResponseDto>.Success(await ProductAppService.UpdateAsync(value.Id, value));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize(ProductPermission.Product.Delete)]
        public async Task<ServiceResponse<bool>> Delete(string id)
        {
            await ProductAppService.DeleteAsync(id);
            return ServiceResponse<bool>.Success(true);
        }


        /// <summary>
        /// 删除
        /// </summary>
        [HttpPost("save")]
        [Authorize(ProductPermission.Product.Create)]
        public async Task<ServiceResponse<bool>> Save(CreateProductCommand cmd)
        {
            await Mediator.Send(cmd);
            return ServiceResponse<bool>.Success(true);
        }


    }
}
