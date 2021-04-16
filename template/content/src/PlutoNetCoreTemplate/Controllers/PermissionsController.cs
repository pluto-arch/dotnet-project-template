namespace PlutoNetCoreTemplate.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Dtos.Permission;
    using Application.Permissions;
    using Infrastructure.Commons;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;


    [Route("api/permission")]
    [ApiController]
    public class PermissionsController: BaseController<PermissionsController>
    {
        private readonly IPermissionAppService _permissionAppService;


        public PermissionsController(IMediator mediator, ILogger<PermissionsController> logger, IPermissionAppService permissionAppService) : base(mediator, logger)
        {
            _permissionAppService = permissionAppService;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="providerName">提供者名称 eg. Role</param>
        /// <param name="providerKey">提供者值 eg. admin</param>
        /// <returns></returns>
        [HttpGet]
        public  async Task<ServiceResponse<PermissionListResponseModel>> GetAsync(string providerName, string providerKey)
        {
            var res= await _permissionAppService.GetAsync(providerName, providerKey);
            return ServiceResponse<PermissionListResponseModel>.Success(res);
        }


        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("getList")]
        public ServiceResponse<List<PermissionGroupDefinition>> GetListAsync()
        {
            var res= _permissionAppService.GetListAsync();
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
        public  async Task<ServiceResponse<bool>> UpdateAsync(string providerName, string providerKey, IEnumerable<PermissionUpdateRequestModel> model)
        {
            await _permissionAppService.UpdateAsync(providerName, providerKey, model);
            return ServiceResponse<bool>.Success(true);
        }


    }
}