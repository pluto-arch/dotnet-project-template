namespace PlutoNetCoreTemplate.Application.Permissions
{
    public class ProductPermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        /// <inheritdoc />
        public void Define(PermissionDefinitionContext context)
        {
            /*
             * can read from database
             */
            var productGroup = context.AddGroup(ProductPermission.GroupName, "产品管理");
            var userPermissionManager = productGroup.AddPermission(ProductPermission.Product.Default, "产品");
            userPermissionManager.AddChild(ProductPermission.Product.Create, "创建");
            userPermissionManager.AddChild(ProductPermission.Product.Edit, "编辑");
            userPermissionManager.AddChild(ProductPermission.Product.Delete, "删除");


            var rolePermissionManager = productGroup.AddPermission(ProductPermission.Device.Default, "设备");
            rolePermissionManager.AddChild(ProductPermission.Device.Create, "创建");
            rolePermissionManager.AddChild(ProductPermission.Device.Edit, "编辑");
            rolePermissionManager.AddChild(ProductPermission.Device.Delete, "删除");
        }
    }
}