namespace PlutoNetCoreTemplate.Domain.Services.PermissionGrant
{
    using Aggregates.PermissionGrant;
    using Aggregates.SystemAggregate;
    using Aggregates.TenantAggregate;

    using Microsoft.Extensions.DependencyInjection;

    using Repositories;

    using SeedWork;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PermissionGrantDataSeedProvider : IDataSeedProvider
    {
        private readonly IPermissionGrantRepository _repository;
        private readonly IRepository<PermissionGroupDefinition> _permissionGroup;
        private readonly IRepository<PermissionDefinition> _permission;
        private readonly ICurrentTenant _currentTenant;

        public PermissionGrantDataSeedProvider(
            IPermissionGrantRepository repository,
            ICurrentTenant currentTenant,
            IRepository<PermissionDefinition> permission,
            IRepository<PermissionGroupDefinition> permissionGroup)
        {
            _repository = repository;
            _currentTenant = currentTenant;
            _permission = permission;
            _permissionGroup = permissionGroup;
        }

        /// <inheritdoc />
        public int Sorts => 100;

        /// <inheritdoc />
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            (string id, string name)[] tenantIds = new (string id, string name)[]
            {
                ("T20210602000001","租户一"),
                ("T20210602000002","租户二"),
                ("T20210602000003","租户三")
            };

            var permissions = new Dictionary<string, List<string>>
            {
                {"ProductManager",new List<string>
                {
                    "ProductManager.Products",
                    "ProductManager.Products.Create",
                    "ProductManager.Products.Edit",
                    "ProductManager.Products.Delete",

                    "ProductManager.Devices",
                    "ProductManager.Devices.Create",
                    "ProductManager.Devices.Edit",
                    "ProductManager.Devices.Delete",
                }},
                {"TenantManager",new List<string>
                {
                    "TenantManager.Tenant",
                    "TenantManager.Tenant.Create",
                    "TenantManager.Tenant.Edit",
                    "TenantManager.Tenant.Delete",
                }},
                {"PermissionManager",new List<string>
                {
                    "PermissionManager.PermissionGroup",
                    "PermissionManager.PermissionGroup.Create",
                    "PermissionManager.PermissionGroup.Edit",
                    "PermissionManager.PermissionGroup.Delete",

                    "PermissionManager.Permission",
                    "PermissionManager.Permission.Create",
                    "PermissionManager.Permission.Edit",
                    "PermissionManager.Permission.Delete",
                }}
            };
            if (!_permissionGroup.Any())
            {
                var group = new List<PermissionGroupDefinition>();
                foreach (var item in permissions)
                {
                    var permission = new PermissionGroupDefinition
                    {
                        Name = item.Key,
                        DisplayName = item.Key,
                        CreateTime = DateTime.Now,
                    };
                    permission.Permissions = new List<PermissionDefinition>();
                    foreach (var p in item.Value)
                    {
                        permission.Permissions.Add(new PermissionDefinition(p, p));
                    }
                    group.Add(permission);
                }
                await _permissionGroup.InsertAsync(group, true);
            }


            var tenantProvider = serviceProvider.GetRequiredService<ITenantProvider>();
            var rep=serviceProvider.GetRequiredService<IPermissionGrantRepository>();
            TenantInfo t = null;
            foreach (var (id, _) in tenantIds)
            {
                t = await tenantProvider.InitTenant(id);
                using (_currentTenant.Change(t))
                {
                    if (rep.Any())
                    {
                        continue;
                    }

                    foreach (var item in permissions)
                    {
                        foreach (var value in item.Value)
                        {
                            await rep.InsertAsync(new PermissionGrant
                            {
                                Name = value,
                                ProviderName = "role",
                                ProviderKey = "admin",
                            });
                        }
                    }
                    await rep.Uow.SaveChangesAsync();
                }
            }
        }
    }
}