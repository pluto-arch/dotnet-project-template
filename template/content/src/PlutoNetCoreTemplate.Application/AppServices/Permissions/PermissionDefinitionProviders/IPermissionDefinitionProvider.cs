namespace PlutoNetCoreTemplate.Application.AppServices.Permissions
{
    public interface IPermissionDefinitionProvider
    {
        void Define(PermissionDefinitionContext context);
    }
}