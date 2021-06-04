## 迁移命令

```
-- PlutoNetTemplateDbContext
Add-Migration InitialCreate -Context PlutoNetTemplateDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure
Update-Database -Context PlutoNetTemplateDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure


-- TenantDbContext
Add-Migration InitialCreate -Context TenantDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure
Update-Database -Context TenantDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure

```

### 使用连接字符串迁移
```
Update-Database -Context PlutoNetTemplateDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure -Connection "Server=127.0.0.1,1433;Database=Pnct_T20210602000002;User Id=sa;Password=970307Lbx$;Trusted_Connection = False;"
```