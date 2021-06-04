namespace PlutoNetCoreTemplate.Application.AppServices.TenantAppServices
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Aggregates.TenantAggregate;
    using Domain.SeedWork;
    using Domain.Services.TenantDomainService;
    using Microsoft.EntityFrameworkCore;
    using Models.TenantModels;

    public class TenantAppService:ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly IMapper _mapper;

        public TenantAppService(TenantManager tenantManager, IMapper mapper)
        {
            _tenantManager = tenantManager;
            _mapper = mapper;
        }

        public async Task<List<TenantModel>> GetListAsync()
        {
            var entities = await _tenantManager.GetListAsync();
            return _mapper.Map<List<TenantModel>>(entities);
        }
    }
}