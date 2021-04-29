namespace PlutoNetCoreTemplate.Application.AppServices.TenantAppServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.TenantModels;

    public interface ITenantAppService
    {
        Task<List<TenantModel>> GetListAsync();
    }
}