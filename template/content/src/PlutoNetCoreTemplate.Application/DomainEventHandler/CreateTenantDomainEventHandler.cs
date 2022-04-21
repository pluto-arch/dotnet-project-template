namespace PlutoNetCoreTemplate.Application.DomainEventHandler
{
    using Domain.Repositories;
    using Infrastructure.EntityFrameworkCore;
    using Microsoft.Data.SqlClient;
    using PlutoNetCoreTemplate.Domain.Aggregates.PermissionGrant;
    using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
    using PlutoNetCoreTemplate.Domain.Events.Tenants;
    using PlutoNetCoreTemplate.Domain.UnitOfWork;

    /// <summary>
    /// 创建租户后的事件处理程序
    /// </summary>
    public class CreateTenantDomainEventHandler : INotificationHandler<CreateTenantDomainEvent>
    {
        private readonly ILogger<CreateTenantDomainEventHandler> _logger;
        private readonly ICurrentTenant _currentTenant;
        private readonly IConfiguration cfg;
        private readonly IRepository<PermissionGrant> _permissionGrants;
        private readonly IUnitOfWork<DeviceCenterDbContext> _uowOfWork;

        public CreateTenantDomainEventHandler(
            ILogger<CreateTenantDomainEventHandler> logger,
            ICurrentTenant currentTenant,
            IConfiguration config, IRepository<PermissionGrant> permissionGrants,
            IUnitOfWork<DeviceCenterDbContext> uowOfWork)
        {
            _logger = logger;
            _currentTenant = currentTenant;
            cfg = config;
            _permissionGrants = permissionGrants;
            _uowOfWork = uowOfWork;
        }


        public async Task Handle(CreateTenantDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(10000, cancellationToken);
                _logger.LogInformation("接受到创建租户领域事件，开始初始化租户 {@notification} 的基础数据", notification);
                if (!notification.IsShareDatabase)
                {
                    return;
                }
                _logger.LogInformation("开始初始化租户{tenantId}的数据库", notification.TenantId);


                var dbName = $"Pnct_{notification.TenantId.Id}";

                // TODO ：tenant is already inited, shoud skip this script。this is only for example
                await using (var conn = new SqlConnection(cfg.GetConnectionString("InitDb")))
                {
                    await conn.OpenAsync(cancellationToken);
                    SqlCommand cmd = new($"USE master;CREATE DATABASE {dbName};", conn);
                    await cmd.ExecuteNonQueryAsync(cancellationToken);
                    await conn.CloseAsync();
                }

                var tenInfo = new TenantInfo(notification.TenantId.Id, notification.TenantId.Name)
                {
                    ConnectionStrings = notification.TenantId.ConnectionStrings.ToDictionary(k => k.Name, v => v.Value)
                };
                using (_currentTenant.Change(tenInfo))
                {
                    await _uowOfWork.Context.Database.EnsureCreatedAsync(cancellationToken);
                    _logger.LogInformation("初始化租户{tenantId}的数据库成功，开始初始化管理员权限数据", notification.TenantId);
                    await InitAdminPermission(cancellationToken);
                    await _uowOfWork.SaveChangesAsync(cancellationToken);

                    // try to change parent scope entity value
                    notification.TenantId.Name += "123123";
                }
                _logger.LogInformation("初始化租户{tenantId}的数据成功", notification.TenantId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "初始化租户数据失败");
                // TODO 记录事件到本地事件表
            }
        }

        private async Task InitAdminPermission(CancellationToken cancellationToken)
        {
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
            if (!(await _permissionGrants.AnyAsync(cancellationToken)))
            {
                foreach (var item in permissions)
                {
                    foreach (var value in item.Value)
                    {
                        await _permissionGrants.InsertAsync(new PermissionGrant
                        {
                            Name = value,
                            ProviderName = "role",
                            ProviderKey = "admin",
                        }, cancellationToken: cancellationToken);
                    }
                }
            }
        }
    }
}
