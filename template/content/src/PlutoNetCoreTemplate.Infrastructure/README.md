## 迁移命令

```
-- PlutoNetTemplateDbContext
Add-Migration InitialCreate -Context PlutoNetTemplateDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure
Update-Database -Context PlutoNetTemplateDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure


-- SystemDbContext
Add-Migration InitialCreate -Context SystemDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure
Update-Database -Context SystemDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure

```

### 使用连接字符串迁移
```
Update-Database -Context PlutoNetTemplateDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure -Connection "Server=127.0.0.1,1433;Database=Pnct_T20210602000002;User Id=sa;Password=970307lBX;Trusted_Connection = False;"
```