namespace PlutoNetCoreTemplate.Application.Permissions
{

    /// <summary>
    /// 系统权限管理定义
    /// </summary>
    public static class SystemPermissions
    {
        public const string GroupName = "PermissionManager";

        public static class Permissions
        {
            public const string Default = GroupName + ".Permissions";
            public const string Get = Default + ".Get";
            public const string Edit = Default + ".Edit";
        }
    }


    public class SystemPermissionDefinitionProvider : IPermissionDefinitionProvider
    {


        //private readonly IStringLocalizer _localizer;

        //public SystemPermissionDefinitionProvider(IStringLocalizerFactory factory)
        //{
        //    _localizer = factory.Create("Welcome", Assembly.GetExecutingAssembly().FullName ?? string.Empty);
        //}



        public void Define(PermissionDefinitionContext context)
        {
            var productGroup = context.AddGroup(SystemPermissions.GroupName, "SystemPermission:PermissionManager");

            var productManagement = productGroup.AddPermission(SystemPermissions.Permissions.Default, "SystemPermission:PermissionStore.Permissions");

            productManagement.AddChild(SystemPermissions.Permissions.Get, "SystemPermission:PermissionManager.Permissions.Get");
            productManagement.AddChild(SystemPermissions.Permissions.Edit, "SystemPermission:PermissionManager.Permissions.Edit");
        }
    }


}