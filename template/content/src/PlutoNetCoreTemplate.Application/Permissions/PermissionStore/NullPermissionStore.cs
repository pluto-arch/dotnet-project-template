namespace PlutoNetCoreTemplate.Application.Permissions
{
    using System.Diagnostics.CodeAnalysis;

    public class NullPermissionStore : IPermissionStore
    {
        /// <inheritdoc />
        public Task<bool> IsGrantedAsync([NotNull] string name, [MaybeNull] string providerName, [MaybeNull] string providerKey)
        {
            return Task.FromResult(true);
        }

        public Task<MultiplePermissionGrantResult> IsGrantedAsync([NotNull] string[] names, [MaybeNull] string providerName, [MaybeNull] string providerKey)
        {
            return Task.FromResult(new MultiplePermissionGrantResult(names, PermissionGrantResult.Prohibited));
        }
    }
}