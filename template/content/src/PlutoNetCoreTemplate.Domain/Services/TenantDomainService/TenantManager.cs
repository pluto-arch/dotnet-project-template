namespace PlutoNetCoreTemplate.Domain.Services.TenantDomainService
{
    using Aggregates.TenantAggregate;

    using Microsoft.EntityFrameworkCore;

    using SeedWork;

    using System.Collections.Generic;
    using System.Threading.Tasks;


    /// <summary>
    /// 租户管理
    /// </summary>
    public class TenantManager
    {
        private readonly ISystemBaseRepository<Tenant> _tenants;

        public TenantManager(ISystemBaseRepository<Tenant> tenants)
        {
            _tenants = tenants;
        }

        /// <summary>
        /// 获取所有租户
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tenant>> GetListAsync()
        {
            return await _tenants.ToListAsync();
        }

        /// <summary>
        /// 获取租户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Tenant> GetAsync(string id)
        {
            return await _tenants.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}