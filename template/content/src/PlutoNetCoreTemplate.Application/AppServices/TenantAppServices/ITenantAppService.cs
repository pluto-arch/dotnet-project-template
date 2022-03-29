namespace PlutoNetCoreTemplate.Application.AppServices.TenantAppServices
{
    using Models.TenantModels;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITenantAppService
    {
        Task<List<TenantDto>> GetListAsync();

        Task<TenantDto> CreateAsync();
    }
}