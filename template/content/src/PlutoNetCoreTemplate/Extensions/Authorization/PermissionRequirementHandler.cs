namespace PlutoNetCoreTemplate.Extensions
{
    using System.Threading.Tasks;
    using Application.Permissions;
    using Microsoft.AspNetCore.Authorization;

    public class PermissionRequirementHandler: AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionChecker _permissionChecker;

        public PermissionRequirementHandler(IPermissionChecker permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }


        /// <inheritdoc />
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (await _permissionChecker.IsGrantedAsync(context.User, requirement.PermissionName))
            {
                context.Succeed(requirement);
            }
        }
    }
}