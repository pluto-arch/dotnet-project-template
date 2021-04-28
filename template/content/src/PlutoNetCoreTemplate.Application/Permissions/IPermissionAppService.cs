namespace PlutoNetCoreTemplate.Application.Permissions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Models.PermissionModels;

    public interface IPermissionAppService
    {
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="providerName">提供者名称 eg. Role</param>
        /// <param name="providerKey">提供者值 eg. admin</param>
        /// <returns></returns>
        Task<PermissionListResponseModel> GetAsync([NotNull] string providerName, [NotNull] string providerKey);

        /// <summary>
        /// 更新授权
        /// </summary>
        /// <param name="providerName">提供者名称 eg. Role</param>
        /// <param name="providerKey">提供者值 eg. admin</param>
        /// <param name="requestModels"></param>
        /// <returns></returns>
        Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, IEnumerable<PermissionUpdateRequestModel> requestModels);


        List<PermissionGroupDefinition> GetListAsync();
    }
}