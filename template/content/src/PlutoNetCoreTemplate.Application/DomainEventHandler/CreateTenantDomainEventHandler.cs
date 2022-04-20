namespace PlutoNetCoreTemplate.Application.DomainEventHandler
{
    using Infrastructure.EntityFrameworkCore;

    using MediatR;

    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using PlutoNetCoreTemplate.Domain.Aggregates.PermissionGrant;
    using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
    using PlutoNetCoreTemplate.Domain.Events.Tenants;
    using PlutoNetCoreTemplate.Domain.UnitOfWork;

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Repositories;

    /// <summary>
    /// 创建租户后的事件处理程序
    /// </summary>
    public class CreateTenantDomainEventHandler : INotificationHandler<CreateTenantDomainEvent>
    {
        private readonly ILogger<CreateTenantDomainEventHandler> _logger;
        private readonly ICurrentTenant _currentTenant;
        private readonly IConfiguration cfg;
        private readonly IRepository<PermissionGrant> _permissionGrants;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateTenantDomainEventHandler(
            ILogger<CreateTenantDomainEventHandler> logger, 
            ICurrentTenant currentTenant, 
            IConfiguration config,IServiceScopeFactory scopeFactory, IRepository<PermissionGrant> permissionGrants)
        {
            _logger = logger;
            _currentTenant = currentTenant;
            cfg = config;
            _permissionGrants = permissionGrants;
            _scopeFactory = scopeFactory;
        }
        

        public async Task Handle(CreateTenantDomainEvent notification, CancellationToken cancellationToken)
        {
            using var scoped = _scopeFactory.CreateScope();
            try
            {
                var _unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork<DeviceCenterDbContext>>();
                var _permissionGrantsRepo = scoped.ServiceProvider.GetRequiredService<IRepository<PermissionGrant>>();
                await Task.Delay(10000, cancellationToken);
                _logger.LogInformation("接受到创建租户领域事件，开始初始化租户 {@notification} 的基础数据",notification);
                if (!notification.IsShareDatabase)
                {
                    return;
                }
                _logger.LogInformation("开始初始化租户{tenantId}的数据库", notification.TenantId);
                var dbName = $"Pnct_{notification.TenantId}";
                await using (var conn = new SqlConnection(cfg.GetConnectionString("InitDb")))
                using (_currentTenant.Change(new TenantInfo(notification.TenantId,"")))
                {
                    await conn.OpenAsync(cancellationToken);
                    SqlCommand cmd = new($"USE master;CREATE DATABASE {dbName};", conn);
                    await cmd.ExecuteNonQueryAsync(cancellationToken);
                    await conn.CloseAsync();
                    if (_unitOfWork.Context == null)
                    {
                        throw new NullReferenceException("can not found any db context for init");
                    }
                    await _unitOfWork.Context.Database.EnsureCreatedAsync(cancellationToken);

                    _logger.LogInformation("初始化租户{tenantId}的数据库成功，开始初始化管理员权限数据",notification.TenantId);
                    // 管理员权限
                    await InitAdminPermission(_permissionGrants, cancellationToken);
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("初始化租户{tenantId}的数据成功", notification.TenantId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "初始化租户数据失败");
                // TODO 记录事件到本地事件表
            }
        }

        private async Task InitAdminPermission(IRepository<PermissionGrant> _permissionGrantsRepo, CancellationToken cancellationToken)
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
            if (!(await _permissionGrantsRepo.AnyAsync(cancellationToken)))
            {
                foreach (var item in permissions)
                {
                    foreach (var value in item.Value)
                    {
                        await _permissionGrantsRepo.InsertAsync(new PermissionGrant
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
