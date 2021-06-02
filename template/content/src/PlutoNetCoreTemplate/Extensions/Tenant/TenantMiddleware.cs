using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;

namespace PlutoNetCoreTemplate.Api.Extensions.Tenant
{
    using Constants;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;

    public class TenantMiddleware : IMiddleware
    {
        const string TenantId = "tenant_id";


        private readonly ICurrentTenant _currentTenant;

        public TenantMiddleware(ICurrentTenant currentTenant) => _currentTenant = currentTenant;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string tenantIdString = ResolveTenantId(context);
            if (string.IsNullOrEmpty(tenantIdString))
            {
                await next(context);
            }
            else
            {
                using (_currentTenant.Change(tenantIdString))
                {
                    await next(context);
                }
            }
        }



        protected virtual string ResolveTenantId(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue(TenantClaimTypes.TenantId, out var headerValues))
            {
                return headerValues.First();
            }

            if (httpContext.Request.Query.TryGetValue(TenantClaimTypes.TenantId, out var queryValues))
            {
                return queryValues.First();
            }

            if (httpContext.Request.Cookies.TryGetValue(TenantClaimTypes.TenantId, out var cookieValue))
            {
                return cookieValue;
            }

            if (httpContext.Request.RouteValues.TryGetValue(TenantClaimTypes.TenantId, out var routeValue))
            {
                return routeValue?.ToString();
            }

            return httpContext.User.FindFirst(TenantClaimTypes.TenantId)?.Value;
        }

    }
}
