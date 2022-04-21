namespace PlutoNetCoreTemplate.Api.Controllers
{
    using Application.AppServices.TenantAppServices;
    using Application.Models.TenantModels;
    using Application.Permissions;

    /// <summary>
    /// 租户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(TenantPermission.Tenant.Default)]
    public class TenantsController : BaseController<PermissionsController>
    {
        private ITenantAppService TenantAppService => LazyGetRequiredService<ITenantAppService>();

        public TenantsController(ILazyLoadServiceProvider lazyLoad) : base(lazyLoad)
        {
        }
        /// <summary>
        /// 获取租户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getList")]
        public async Task<ServiceResponse<List<TenantDto>>> GetListAsync()
        {
            var res = await TenantAppService.GetListAsync();
            return ServiceResponse<List<TenantDto>>.Success(res);
        }


        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ServiceResponse<TenantDto>> CreateAsync()
        {
            var res = await TenantAppService.CreateAsync();
            return ServiceResponse<TenantDto>.Success(res);
        }


    }
}