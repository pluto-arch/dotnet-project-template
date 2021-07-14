namespace PlutoNetCoreTemplate.Domain.Services.PermissionGrant
{
    using Aggregates.PermissionGrant;
    using Aggregates.SystemAggregate;
    using Aggregates.TenantAggregate;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using SeedWork;

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PermissionGrantDataSeedProvider : IDataSeedProvider
    {
        private readonly IPermissionGrantRepository _repository;
        private readonly ISystemBaseRepository<PermissionGroupDefinition> _permissionGroup;
        private readonly ISystemBaseRepository<PermissionDefinition> _permission;
        private readonly ICurrentTenant _currentTenant;

        public PermissionGrantDataSeedProvider(IPermissionGrantRepository repository, ICurrentTenant currentTenant, ISystemBaseRepository<PermissionDefinition> permission, ISystemBaseRepository<PermissionGroupDefinition> permissionGroup)
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
            string[] tenantIds = new[] { "T20210602000001", "T20210602000002" };
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
            if (!(await _permissionGroup.AnyAsync()))
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



            foreach (var tenantId in tenantIds)
            {
                using (_currentTenant.Change(tenantId, "租户一", out var scope))
                {
                    var productRepository = scope.ServiceProvider.GetService<IPermissionGrantRepository>();
                    if (await productRepository.AnyAsync())
                    {
                        continue;
                    }

                    foreach (var item in permissions)
                    {
                        foreach (var value in item.Value)
                        {
                            await productRepository.InsertAsync(new PermissionGrant
                            {
                                Name = value,
                                ProviderName = "role",
                                ProviderKey = "admin",
                            });
                        }
                    }
                    await productRepository.Uow.SaveChangesAsync();
                }
            }
        }
    }
}