namespace PlutoNetCoreTemplate.Api.Controllers
{
    using Application.Models.PermissionModels;
    using Application.Permissions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.SeedWork;

    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : BaseController<PermissionsController>
    {
        private IPermissionAppService PermissionAppService=>LazyGetRequiredService<IPermissionAppService>();


        public PermissionsController(ILazyLoadServiceProvider lazyLoad) : base(lazyLoad)
        {
        }


        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="providerName">提供者名称 eg. Role</param>
        /// <param name="providerKey">提供者值 eg. admin</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(SystemPermissions.Permissions.Get)]
        public async Task<ServiceResponse<PermissionListResponseModel>> GetAsync(string providerName, string providerKey)
        {
            var res = await PermissionAppService.GetAsync(providerName, providerKey);
            return ServiceResponse<PermissionListResponseModel>.Success(res);
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("getList")]
        [Authorize(SystemPermissions.Permissions.Get)]
        public ServiceResponse<List<PermissionGroupDefinition>> GetListAsync()
        {
            var res = PermissionAppService.GetListAsync();
            return ServiceResponse<List<PermissionGroupDefinition>>.Success(res);
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerKey"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(SystemPermissions.Permissions.Edit)]
        public async Task<ServiceResponse<bool>> UpdateAsync(string providerName, string providerKey, IEnumerable<PermissionUpdateRequestModel> model)
        {
            await PermissionAppService.UpdateAsync(providerName, providerKey, model);
            return ServiceResponse<bool>.Success(true);
        }

    }
}