namespace PlutoNetCoreTemplate.Application.Permissions
{
    using System.Security.Claims;

    /// <summary>
    /// 角色级别的权限值检测提供程序
    /// </summary>
    public class RolePermissionValueProvider : IPermissionValueProvider
    {

        public const string ProviderName = "Role";

        public string Name => ProviderName;


        private readonly IPermissionStore _permissionStore;

        public RolePermissionValueProvider(IPermissionStore permissionStore)
        {
            _permissionStore = permissionStore;
        }



        /// <inheritdoc />
        public async Task<PermissionGrantResult> CheckAsync(ClaimsPrincipal principal, PermissionDefinition permission)
        {
            var roles = principal?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
            if (roles == null || !roles.Any())
            {
                return PermissionGrantResult.Undefined;
            }

            foreach (var role in roles)
            {
                if (await _permissionStore.IsGrantedAsync(permission.Name, Name, role))
                {
                    return PermissionGrantResult.Granted;
                }
            }

            return PermissionGrantResult.Undefined;
        }

        /// <inheritdoc />
        public async Task<MultiplePermissionGrantResult> CheckAsync(ClaimsPrincipal principal, List<PermissionDefinition> permissions)
        {
            var permissionNames = permissions.Select(x => x.Name).ToList();
            var result = new MultiplePermissionGrantResult(permissionNames.ToArray());

            var roles = principal?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

            if (roles is null || !roles.Any())
            {
                return result;
            }

            foreach (var role in roles)
            {
                var multipleResult = await _permissionStore.IsGrantedAsync(permissionNames.ToArray(), Name, role);
                var ddd = multipleResult.Result.Where(grantResult =>
                    result.Result.ContainsKey(grantResult.Key) &&
                    result.Result[grantResult.Key] == PermissionGrantResult.Undefined &&
                    grantResult.Value != PermissionGrantResult.Undefined);
                foreach (var grantResult in ddd)
                {
                    result.Result[grantResult.Key] = grantResult.Value;
                    permissionNames.RemoveAll(x => x == grantResult.Key);
                }

                if (result.AllGranted || result.AllProhibited)
                {
                    break;
                }

            }

            return result;
        }
    }
}