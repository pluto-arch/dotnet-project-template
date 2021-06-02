## 迁移命令

```
Add-Migration InitialCreate -Context PlutoNetTemplateDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure

Update-Database -Context PlutoNetTemplateDbContext -Project PlutoNetCoreTemplate.Infrastructure -StartupProject PlutoNetCoreTemplate.Infrastructure
```