namespace PlutoNetCoreTemplate.Application.AppServices.Permissions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IPermissionChecker
    {
        Task<bool> IsGrantedAsync([NotNull] string name);

        Task<bool> IsGrantedAsync([MaybeNull] ClaimsPrincipal claimsPrincipal, [NotNull] string name);

        Task<MultiplePermissionGrantResult> IsGrantedAsync([NotNull] string[] names);

        Task<MultiplePermissionGrantResult> IsGrantedAsync([MaybeNull] ClaimsPrincipal claimsPrincipal, [NotNull] string[] names);
    }
}