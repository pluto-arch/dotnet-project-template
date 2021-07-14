namespace PlutoNetCoreTemplate.Application.AppServices.TenantAppServices
{
    using AutoMapper;

    using Domain.Services.TenantDomainService;

    using Models.TenantModels;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TenantAppService : ITenantAppService
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