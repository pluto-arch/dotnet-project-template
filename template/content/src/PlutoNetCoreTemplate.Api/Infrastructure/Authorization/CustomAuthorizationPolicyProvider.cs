﻿namespace PlutoNetCoreTemplate.Api
{
    using Application.Permissions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.Extensions.Options;

    using System;
    using System.Threading.Tasks;

    public class CustomAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {

        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        /// <inheritdoc />
        public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IPermissionDefinitionManager permissionDefinitionManager) : base(options)
        {
            _permissionDefinitionManager = permissionDefinitionManager;
        }


        /// <inheritdoc />
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            AuthorizationPolicy policy = await base.GetPolicyAsync(policyName);

            if (policy is not null)
            {
                return policy;
            }

            var permission = _permissionDefinitionManager.GetOrNull(policyName);

            if (permission is not null)
            {
                var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());
                policyBuilder.Requirements.Add(new OperationAuthorizationRequirement { Name = policyName });
                return policyBuilder.Build();
            }

            return null;

        }
    }
}