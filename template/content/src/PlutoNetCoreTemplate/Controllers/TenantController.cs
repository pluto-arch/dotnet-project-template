namespace PlutoNetCoreTemplate.Controllers
{
    using Domain.Aggregates.TenantAggregate;
    using Infrastructure.Commons;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 租户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController: BaseController<PermissionsController>
    {
        private readonly ICurrentTenant _currentTenant;
        public TenantController(IMediator mediator, ILogger<PermissionsController> logger, ICurrentTenant currentTenant) : base(mediator, logger)
        {
            _currentTenant = currentTenant;
        }

        /// <summary>
        /// 获取当前租户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ServiceResponse<string> Get()
        {
            return ServiceResponse<string>.Success(_currentTenant.Id);
        }
    }
}