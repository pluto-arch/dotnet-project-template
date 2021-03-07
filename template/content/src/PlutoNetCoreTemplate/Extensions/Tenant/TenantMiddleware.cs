using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;

namespace PlutoNetCoreTemplate.Extensions.Tenant
{

    public class TenantMiddleware : IMiddleware
    {
        const string TenantId = "tanent_id";


        private readonly ICurrentTenant _currentTenant;

        public TenantMiddleware(ICurrentTenant currentTenant) => _currentTenant = currentTenant;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var tenantIdString = string.Empty;

            if (context.Request.Headers.TryGetValue(TenantId, out var headerTenantIds))
            {
                tenantIdString = headerTenantIds.First();
            }

            if (context.Request.Query.TryGetValue(TenantId, out var queryTenantIds))
            {
                tenantIdString = queryTenantIds.First();
            }

            if (context.Request.Cookies.TryGetValue(TenantId, out var cookieTenantId))
            {
                tenantIdString = cookieTenantId;
            }

            if (context.Request.RouteValues.TryGetValue(TenantId, out var routeTenantId))
            {
                tenantIdString = routeTenantId?.ToString();
            }

            tenantIdString ??= context.User.FindFirst(TenantId)?.Value;

            string currentTenantId = null;

            if (!string.IsNullOrWhiteSpace(tenantIdString))
            {
                currentTenantId = tenantIdString;
            }

            using (_currentTenant.Change(currentTenantId))
            {
                await next(context);
            }
        }

    }
}
