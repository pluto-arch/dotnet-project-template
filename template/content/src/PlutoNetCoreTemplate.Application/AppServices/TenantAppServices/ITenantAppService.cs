namespace PlutoNetCoreTemplate.Application.AppServices.TenantAppServices
{
    using Models.TenantModels;

    public interface ITenantAppService
    {
        Task<List<TenantDto>> GetListAsync();

        Task<TenantDto> CreateAsync();
    }
}