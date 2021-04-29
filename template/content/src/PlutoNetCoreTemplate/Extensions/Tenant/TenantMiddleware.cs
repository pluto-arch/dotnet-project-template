using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;

namespace PlutoNetCoreTemplate.Extensions.Tenant
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using PlutoData;
    using Serilog;

    public class TenantMiddleware : IMiddleware
    {
        const string TenantId = "tenant_id";


        private readonly ICurrentTenant _currentTenant;

        public TenantMiddleware(ICurrentTenant currentTenant) => _currentTenant = currentTenant;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string tenantIdString;

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

            tenantIdString = context.User.Claims.FirstOrDefault(x=>x.Type==TenantId)?.Value;

            string currentTenantId = null;

            if (!string.IsNullOrWhiteSpace(tenantIdString))
            {
                currentTenantId = tenantIdString;
                Log.Logger.Information($"当前租户：{currentTenantId}");
            }

            using (_currentTenant.Change(currentTenantId))
            {
                await next(context);
            }
        }

    }
}
