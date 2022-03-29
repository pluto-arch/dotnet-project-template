namespace PlutoNetCoreTemplate.Application.Permissions
{
    public static class TenantPermission
    {
        public const string GroupName = "TenantManager";


        public static class Tenant
        {
            public const string Default = GroupName + ".Tenant";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

    }


    public class TenantPermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        /// <inheritdoc />
        public void Define(PermissionDefinitionContext context)
        {
            /*
             * can read from database
             */
            var productGroup = context.AddGroup(TenantPermission.GroupName, "TenantPermission.TenantManager");
            var userPermissionManager = productGroup.AddPermission(TenantPermission.Tenant.Default, "TenantPermission.TenantManager.Tenant");
            userPermissionManager.AddChild(TenantPermission.Tenant.Create, "TenantPermission.TenantManager.Tenant.Create");
            userPermissionManager.AddChild(TenantPermission.Tenant.Edit, "TenantPermission.TenantManager.Tenant.Edit");
            userPermissionManager.AddChild(TenantPermission.Tenant.Delete, "TenantPermission.TenantManager.Tenant.Delete");
        }
    }
}