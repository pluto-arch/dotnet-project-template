namespace PlutoNetCoreTemplate.Application.Permissions
{
    /// <summary>
    /// 项目权限定义
    /// </summary>
    public static class ProjectPermissions
    {
        public const string GroupName = "ProjectManager";

        public static class Projects
        {
            public const string Default = GroupName + ".Projects";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }


    public class ProjectPermissionsDefinitionProvider : IPermissionDefinitionProvider
    {
        public void Define(PermissionDefinitionContext context)
        {
            var productGroup = context.AddGroup(ProjectPermissions.GroupName, "ProjectPermissions:ProjectManager");

            var productManagement = productGroup.AddPermission(ProjectPermissions.Projects.Default, "ProjectPermissions:ProjectManager");

            productManagement.AddChild(ProjectPermissions.Projects.Create, "ProjectPermissions:ProjectManager.Creeate");
            productManagement.AddChild(ProjectPermissions.Projects.Edit, "ProjectPermissions:ProjectManager.Edit");
            productManagement.AddChild(ProjectPermissions.Projects.Delete, "ProjectPermissions:ProjectManager.Delete");
        }
    }
}