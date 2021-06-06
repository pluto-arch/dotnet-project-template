namespace PlutoNetCoreTemplate.Application.Permissions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Domain.Aggregates.PermissionGrant;
    using Microsoft.Extensions.Logging;

    public class PermissionStore:IPermissionStore
    {
        private readonly IPermissionGrantRepository _permissionGrantRepository;

        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        private readonly ILogger<PermissionStore> _logger;

        public PermissionStore(IPermissionGrantRepository permissionGrantRepository, IPermissionDefinitionManager permissionDefinitionManager, ILogger<PermissionStore> logger)
        {
            _permissionGrantRepository = permissionGrantRepository;
            _permissionDefinitionManager = permissionDefinitionManager;
            _logger = logger;
        }


        private const string CacheKeyFormat = "pn:{0},pk:{1},n:{2}"; //<object-type>:<id>:<field1>.<field2> Or <object-type>:<id>:<field1>-<field2>

        private readonly ConcurrentDictionary<string,string> permissionCached=new();

        /// <inheritdoc />
        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
        }


        protected virtual async Task<(string Key, bool IsGranted)> GetCacheItemAsync(string name, string providerName, string providerKey)
        {
            var cacheKey = string.Format(CacheKeyFormat, providerName, providerKey, name);
            _logger.LogDebug($"PermissionStore.GetCacheItemAsync: {cacheKey}");
            permissionCached.TryGetValue(cacheKey,out string value);

            if (value is not null)
            {
                _logger.LogDebug($"Found in the cache: {cacheKey}");
                return (cacheKey, Convert.ToBoolean(value));
            }
            _logger.LogDebug($"Not found in the cache: {cacheKey}");
            return await SetCacheItemsAsync(providerName, providerKey, name);
        }


        protected virtual async Task<(string Key, bool IsGranted)> SetCacheItemsAsync(string providerName, string providerKey, string currentName)
        {
            var permissions = _permissionDefinitionManager.GetPermissions();
            _logger.LogDebug($"Getting all granted permissions from the repository for this provider name,key: {providerName},{providerKey}");
            var grantedPermissionsHashSet = new HashSet<string>((await _permissionGrantRepository.GetListAsync(providerName, providerKey)).Select(p => p.Name));
            _logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");
            bool currentResult = false;
            foreach (var permission in permissions)
            {
                var isGranted = grantedPermissionsHashSet.Contains(permission.Name);
                permissionCached.TryAdd(string.Format(CacheKeyFormat, providerName, providerKey, permission.Name), isGranted.ToString());
                if (permission.Name == currentName)
                {
                    currentResult = isGranted;
                }
            }
            _logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");
            return (string.Format(CacheKeyFormat, providerName, providerKey, currentName), currentResult);
        }






        /// <inheritdoc />
        public async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            MultiplePermissionGrantResult result = new ();

            if (names.Length == 1)
            {
                var name = names.First();
                result.Result.Add(name, await IsGrantedAsync(names.First(), providerName, providerKey) ? PermissionGrantResult.Granted : PermissionGrantResult.Undefined);
                return result;
            }

            var cacheItems = await GetCacheItemsAsync(names, providerName, providerKey);

            foreach (var (Key, IsGranted) in cacheItems)
            {
                result.Result.Add(GetPermissionInfoFormCacheKey(Key).Name, IsGranted ? PermissionGrantResult.Granted : PermissionGrantResult.Undefined);
            }

            return result;
        }

        protected virtual async Task<List<(string Key, bool IsGranted)>> GetCacheItemsAsync(string[] names, string providerName, string providerKey)
        {
            var cacheKeys = names.Select(x => string.Format(CacheKeyFormat, x, providerName, providerKey)).ToList();

            _logger.LogDebug($"PermissionStore.GetCacheItemAsync: {string.Join(",", cacheKeys)}");

            List<(string key, string value)> getCacheItemTasks = new ();

            foreach (string cacheKey in cacheKeys)
            {
                if (permissionCached.TryGetValue(cacheKey, out string value))
                {
                    getCacheItemTasks.Add((cacheKey,value));
                }
            }

            if (getCacheItemTasks.All(x => x.value is not null))
            {
                _logger.LogDebug($"Found in the cache: {string.Join(",", cacheKeys)}");
                return Array.ConvertAll(getCacheItemTasks.ToArray(), i => (i.key, Convert.ToBoolean(i))).ToList();
            }

            var notCacheKeys = getCacheItemTasks.Where(x => x.value is null).Select(x => x.key).ToList();

            _logger.LogDebug($"Not found in the cache: {string.Join(",", notCacheKeys)}");

            return await SetCacheItemsAsync(providerName, providerKey, notCacheKeys);
        }


        protected virtual async Task<List<(string Key, bool IsGranted)>> SetCacheItemsAsync(string providerName, string providerKey, List<string> notCacheKeys)
        {
            var permissions = _permissionDefinitionManager.GetPermissions().Where(x => notCacheKeys.Any(k => GetPermissionInfoFormCacheKey(k).Name == x.Name)).ToList();

            _logger.LogDebug($"Getting not cache granted permissions from the repository for this provider name,key: {providerName},{providerKey}");

            var grantedPermissionsHashSet = new HashSet<string>((await 
                _permissionGrantRepository.GetListAsync(notCacheKeys.Select(k => GetPermissionInfoFormCacheKey(k).Name).ToArray(), providerName, providerKey)).Select(p => p.Name));

            _logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");

            var cacheItems = new List<(string Key, bool IsGranted)>();

            foreach (PermissionDefinition permission in permissions)
            {
                var isGranted = grantedPermissionsHashSet.Contains(permission.Name);
                cacheItems.Add((string.Format(CacheKeyFormat, providerName, providerKey, permission.Name), isGranted));
            }

            foreach ((string key, bool isGranted) in cacheItems)
            {
                permissionCached.TryAdd(key,isGranted.ToString());
            }
            _logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");

            return cacheItems;
        }


        protected virtual (string ProviderName, string ProviderKey, string Name) GetPermissionInfoFormCacheKey(string key)
        {
            string pattern = @"^pn:(?<providerName>.+),pk:(?<providerKey>.+),n:(?<name>.+)$";

            Match match = Regex.Match(key, pattern, RegexOptions.IgnoreCase);

            string providerName = match.Groups["providerName"].Value;
            string providerKey = match.Groups["providerKey"].Value;
            string name = match.Groups["name"].Value;

            return (providerName, providerKey, name);
        }
    }
}