namespace PlutoNetCoreTemplate.Extensions
{
    using System.Threading.Tasks;
    using Application.Permissions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;


    /// <summary>
    /// 细化到操作的处理程序
    /// </summary>
    public class PermissionRequirementHandler: AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly IPermissionChecker _permissionChecker;

        public PermissionRequirementHandler(IPermissionChecker permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }


        /// <inheritdoc />
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            if (await _permissionChecker.IsGrantedAsync(context.User, requirement.Name))
            {
                context.Succeed(requirement);
            }
        }
    }
}