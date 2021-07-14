namespace PlutoNetCoreTemplate.Api.Controllers
{
    using Application.AppServices.TenantAppServices;
    using Application.Models.TenantModels;
    using Application.Permissions;

    using Domain.Aggregates.TenantAggregate;

    using Infrastructure.Commons;

    using MediatR;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 租户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(TenantPermission.Tenant.Default)]
    public class TenantController : BaseController<PermissionsController>
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly ITenantAppService _tenantAppService;
        public TenantController(IMediator mediator, ILogger<PermissionsController> logger, ICurrentTenant currentTenant, ITenantAppService tenantAppService) : base(mediator, logger)
        {
            _currentTenant = currentTenant;
            _tenantAppService = tenantAppService;
        }

        /// <summary>
        /// 获取租户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getList")]
        public async Task<ServiceResponse<List<TenantModel>>> GetListAsync()
        {
            var res = await _tenantAppService.GetListAsync();
            return ServiceResponse<List<TenantModel>>.Success(res);
        }

    }
}