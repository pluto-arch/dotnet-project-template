namespace PlutoNetCoreTemplate.Application.Permissions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IPermissionChecker
    {

        Task<bool> IsGrantedAsync([MaybeNull] ClaimsPrincipal claimsPrincipal, [NotNull] string name);


        Task<MultiplePermissionGrantResult> IsGrantedAsync([MaybeNull] ClaimsPrincipal claimsPrincipal, [NotNull] string[] names);
    }
}