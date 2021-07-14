namespace PlutoNetCoreTemplate.Application.Permissions
{
    using Domain.Aggregates.PermissionGrant;
    using EntityFrameworkCore.Extension.UnitOfWork.Uows;
    using Infrastructure;
    using Models.PermissionModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PermissionAppService : IPermissionAppService
    {
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        private readonly IPermissionGrantRepository _permissionGrantRepository;

        private readonly IUnitOfWork<PlutoNetTemplateDbContext> _uow;


        public PermissionAppService(IPermissionDefinitionManager permissionDefinitionManager, IUnitOfWork<PlutoNetTemplateDbContext> uow)
        {
            _permissionDefinitionManager = permissionDefinitionManager;
            _uow = uow;
            _permissionGrantRepository = _uow.GetRepository<IPermissionGrantRepository>();
        }

        public async Task<PermissionListResponseModel> GetAsync(string providerName, string providerKey)
        {
            var result = new PermissionListResponseModel { DisplayName = providerKey, Groups = new List<PermissionGroupModel>() };

            foreach (var group in _permissionDefinitionManager.GetGroups())
            {
                PermissionGroupModel permissionGroupModel = new()
                {
                    DisplayName = group.DisplayName,
                    Name = group.Name,
                    Permissions = new List<PermissionGrantModel>()
                };
                foreach (PermissionDefinition permission in group.GetPermissionsWithChildren())
                {
                    if (permission.IsEnabled && (!permission.AllowedProviders.Any() ||
                                                 permission.AllowedProviders.Contains(providerName)))
                    {
                        PermissionGrantModel permissionGrantModel = new()
                        {
                            Name = permission.Name,
                            DisplayName = permission.DisplayName,
                            ParentName = permission.Parent?.Name!,
                            AllowedProviders = permission.AllowedProviders
                        };
                        if (permission.AllowedProviders.Any() && !permission.AllowedProviders.Contains(providerName))
                        {
                            throw new ApplicationException($"The permission named {permission.Name} has not compatible with the provider named {providerName}");
                        }
                        if (!permission.IsEnabled)
                        {
                            throw new ApplicationException($"The permission named {permission.Name} is disabled");
                        }
                        PermissionGrant permissionGrant = await _permissionGrantRepository.FindAsync(permission.Name, providerName, providerKey);
                        permissionGrantModel.IsGranted = permissionGrant != null;
                        permissionGroupModel.Permissions.Add(permissionGrantModel);
                    }
                }
                if (permissionGroupModel.Permissions.Any())
                {
                    result.Groups.Add(permissionGroupModel);
                }
            }

            return result;
        }

        public async Task UpdateAsync(string providerName, string providerKey, IEnumerable<PermissionUpdateRequestModel> requestModels)
        {
            foreach (PermissionUpdateRequestModel requestModel in requestModels)
            {
                var permission = _permissionDefinitionManager.Get(requestModel.Name);

                if (permission.AllowedProviders.Any() && !permission.AllowedProviders.Contains(providerName))
                {
                    throw new ApplicationException($"The permission named {permission.Name} has not compatible with the provider named {providerName}");
                }

                if (!permission.IsEnabled)
                {
                    throw new ApplicationException($"The permission named {permission.Name} is disabled");
                }

                PermissionGrant permissionGrant = await _permissionGrantRepository.FindAsync(requestModel.Name, providerName, providerKey);

                if (requestModel.IsGranted && permissionGrant is null)
                {
                    await _permissionGrantRepository.InsertAsync(new PermissionGrant { Name = requestModel.Name, ProviderName = providerName, ProviderKey = providerKey, CreateTime = DateTimeOffset.UtcNow });
                }

                if (!requestModel.IsGranted && permissionGrant is not null)
                {
                    await _permissionGrantRepository.DeleteAsync(permissionGrant);
                }
            }

            await _uow.SaveChangesAsync();
        }


        public List<PermissionGroupDefinition> GetListAsync()
        {
            return _permissionDefinitionManager.GetGroups().ToList();
        }
    }
}