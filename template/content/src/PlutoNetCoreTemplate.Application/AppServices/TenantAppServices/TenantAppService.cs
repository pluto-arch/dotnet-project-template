namespace PlutoNetCoreTemplate.Application.AppServices.TenantAppServices
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Aggregates.TenantAggregate;
    using Domain.SeedWork;
    using Microsoft.EntityFrameworkCore;
    using Models.TenantModels;

    public class TenantAppService:ITenantAppService
    {
        private readonly IPlutoNetCoreTemplateEfRepository<Tenant> _tenantRepository;
        private readonly IMapper _mapper;

        public TenantAppService(IPlutoNetCoreTemplateEfRepository<Tenant> tenantRepository, IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
        }

        public async Task<List<TenantModel>> GetListAsync()
        {
            var entities = await _tenantRepository.Query.ToListAsync();
            return _mapper.Map<List<TenantModel>>(entities);
        }
    }
}