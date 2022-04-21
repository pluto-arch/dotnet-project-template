
using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;

namespace PlutoNetCoreTemplate.Api
{
    using Constants;

    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tenantIdInfo = await ResolveTenantId(context);
            if (tenantIdInfo is null)
            {
                await _next(context);
            }
            else
            {
                var currentTenant = context.RequestServices.GetRequiredService<ICurrentTenant>();
                if (currentTenant == null)
                {
                    await _next(context);
                }
                else
                {
                    using (currentTenant.Reserve(tenantIdInfo))
                    {
                        await _next(context);
                    }
                }
            }
        }


        protected virtual async Task<TenantInfo> ResolveTenantId(HttpContext httpContext)
        {
            string tenantId = string.Empty;
            if (httpContext.Request.Headers.TryGetValue(TenantClaimTypes.TenantId, out var headerValues))
            {
                tenantId = headerValues.First();
            }

            if (httpContext.Request.Query.TryGetValue(TenantClaimTypes.TenantId, out var queryValues))
            {
                tenantId = queryValues.First();
            }

            if (httpContext.Request.Cookies.TryGetValue(TenantClaimTypes.TenantId, out var cookieValue))
            {
                tenantId = cookieValue;
            }

            if (httpContext.Request.RouteValues.TryGetValue(TenantClaimTypes.TenantId, out var routeValue))
            {
                tenantId = routeValue?.ToString();
            }

            if (httpContext.User.FindFirst(TenantClaimTypes.TenantId)?.Value is not null)
            {
                tenantId = httpContext.User.FindFirst(TenantClaimTypes.TenantId)?.Value;
            }

            var tenantProvider = httpContext.RequestServices.GetRequiredService<ITenantProvider>();
            var t = await tenantProvider.InitTenant(tenantId);
            return t;
        }

    }
}
