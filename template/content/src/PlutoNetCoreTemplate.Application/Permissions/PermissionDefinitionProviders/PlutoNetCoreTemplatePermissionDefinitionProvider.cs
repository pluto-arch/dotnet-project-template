namespace PlutoNetCoreTemplate.Application.Permissions
{
    public class PlutoNetCoreTemplatePermissionDefinitionProvider:IPermissionDefinitionProvider
    {
        /// <inheritdoc />
        public void Define(PermissionDefinitionContext context)
        {
            /*
             * can read from database
             */
            var productGroup = context.AddGroup(SystemPermission.GroupName, "系统管理");
            var userPermissionManager = productGroup.AddPermission(SystemPermission.SysUser.Default, "用户");
            userPermissionManager.AddChild(SystemPermission.SysUser.Create, "创建");
            userPermissionManager.AddChild(SystemPermission.SysUser.Edit, "编辑");
            userPermissionManager.AddChild(SystemPermission.SysUser.Delete, "删除");


            var rolePermissionManager = productGroup.AddPermission(SystemPermission.SysRole.Default, "角色");
            rolePermissionManager.AddChild(SystemPermission.SysRole.Create, "创建");
            rolePermissionManager.AddChild(SystemPermission.SysRole.Edit, "编辑");
            rolePermissionManager.AddChild(SystemPermission.SysRole.Delete, "删除");
        }
    }
}