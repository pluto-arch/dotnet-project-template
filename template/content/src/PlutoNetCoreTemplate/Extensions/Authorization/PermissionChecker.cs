namespace PlutoNetCoreTemplate.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Application.Permissions;
    using Microsoft.AspNetCore.Http;

    public class PermissionChecker:IPermissionChecker
    {

        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        private readonly IEnumerable<IPermissionValueProvider> _permissionValueProviders;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionChecker(IPermissionDefinitionManager permissionDefinitionManager, IEnumerable<IPermissionValueProvider> permissionValueProviders, IHttpContextAccessor httpContextAccessor)
        {
            _permissionDefinitionManager = permissionDefinitionManager;
            _permissionValueProviders = permissionValueProviders;
            _httpContextAccessor = httpContextAccessor;
        }


        /// <inheritdoc />
        public async Task<bool> IsGrantedAsync(string name)=> await IsGrantedAsync(_httpContextAccessor.HttpContext!.User, name);

        /// <inheritdoc />
        public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            PermissionDefinition permissionDefinition = _permissionDefinitionManager.Get(name);

            if (!permissionDefinition.IsEnabled)
            {
                return false;
            }

            var isGranted = false;

            foreach (var permissionValueProvider in _permissionValueProviders)
            {
                if (permissionDefinition.AllowedProviders.Any() && !permissionDefinition.AllowedProviders.Contains(permissionValueProvider.Name))
                {
                    continue;
                }

                var result = await permissionValueProvider.CheckAsync(claimsPrincipal, permissionDefinition);

                if (result == PermissionGrantResult.Granted)
                {
                    isGranted = true;
                }
                else if (result == PermissionGrantResult.Prohibited)
                {
                    return false;
                }
            }
            return isGranted;

        }

        /// <inheritdoc />
        public async Task<MultiplePermissionGrantResult> IsGrantedAsync([NotNull]string[] names)=> await IsGrantedAsync(_httpContextAccessor.HttpContext!.User, names);

        /// <inheritdoc />
        public async Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string[] names)
        {
            MultiplePermissionGrantResult result = new ();

            names ??= Array.Empty<string>();

            List<PermissionDefinition> permissionDefinitions = new ();

            foreach (string name in names)
            {
                var permission = _permissionDefinitionManager.Get(name);
                result.Result.Add(name, PermissionGrantResult.Undefined);
                if (permission.IsEnabled)
                {
                    permissionDefinitions.Add(permission);
                }
            }

            foreach (var permissionValueProvider in _permissionValueProviders)
            {
                var pf = permissionDefinitions.Where(x => !x.AllowedProviders.Any() || x.AllowedProviders.Contains(permissionValueProvider.Name)).ToList();

                var multipleResult = await permissionValueProvider.CheckAsync(claimsPrincipal, pf);

                foreach (var grantResult in multipleResult.Result.Where(grantResult => result.Result.ContainsKey(grantResult.Key) && result.Result[grantResult.Key] == PermissionGrantResult.Undefined && grantResult.Value != PermissionGrantResult.Undefined))
                {
                    result.Result[grantResult.Key] = grantResult.Value;
                    permissionDefinitions.RemoveAll(x => x.Name == grantResult.Key);
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