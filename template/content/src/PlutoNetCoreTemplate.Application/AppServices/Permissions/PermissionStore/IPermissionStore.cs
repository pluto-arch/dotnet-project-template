namespace PlutoNetCoreTemplate.Application.AppServices.Permissions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    public interface IPermissionStore
    {
        Task<bool> IsGrantedAsync([NotNull] string name, [MaybeNull] string providerName, [MaybeNull] string providerKey);

        Task<MultiplePermissionGrantResult> IsGrantedAsync([NotNull] string[] names, [MaybeNull] string providerName, [MaybeNull] string providerKey);
    }
}