namespace PlutoNetCoreTemplate.Application.Permissions
{
    public class TenantPermissionDefinitionProvider:IPermissionDefinitionProvider
    {
        /// <inheritdoc />
        public void Define(PermissionDefinitionContext context)
        {
            /*
             * can read from database
             */
            var productGroup = context.AddGroup(TenantPermission.GroupName, "租户管理");
            var userPermissionManager = productGroup.AddPermission(TenantPermission.Tenant.Default, "租户");
            userPermissionManager.AddChild(TenantPermission.Tenant.Create, "创建");
            userPermissionManager.AddChild(TenantPermission.Tenant.Edit, "编辑");
            userPermissionManager.AddChild(TenantPermission.Tenant.Delete, "删除");
        }
    }
}