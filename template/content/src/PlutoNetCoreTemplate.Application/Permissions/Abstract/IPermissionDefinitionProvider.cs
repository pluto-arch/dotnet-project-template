namespace PlutoNetCoreTemplate.Application.Permissions
{
    public interface IPermissionDefinitionProvider
    {
        void Define(PermissionDefinitionContext context);
    }
}