namespace PlutoNetCoreTemplate.Application.AppServices.Permissions.PermissionDefinitionProviders
{
    public class PlutoNetCoreTemplatePermissionDefinitionProvider:IPermissionDefinitionProvider
    {
        /// <inheritdoc />
        public void Define(PermissionDefinitionContext context)
        {
            /*
             * can read from database
             */
            var productGroup = context.AddGroup(PlutoNetCoreTemplatePermission.GroupName, "默认分组");

            var productManagement = productGroup.AddPermission(PlutoNetCoreTemplatePermission.SysUser.Default, "系统用户");

            productManagement.AddChild(PlutoNetCoreTemplatePermission.SysUser.Create, "创建");
            productManagement.AddChild(PlutoNetCoreTemplatePermission.SysUser.Edit, "编辑");
            productManagement.AddChild(PlutoNetCoreTemplatePermission.SysUser.Delete, "删除");
        }
    }
}