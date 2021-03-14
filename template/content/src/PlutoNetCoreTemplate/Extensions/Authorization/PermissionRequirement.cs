namespace PlutoNetCoreTemplate.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Authorization;

    public class PermissionRequirement:IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement([NotNull] string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
            {
                throw new ArgumentException(permissionName);
            }

            PermissionName = permissionName;
        }
    }
}