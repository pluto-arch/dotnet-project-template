namespace PlutoNetCoreTemplate.Api.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    using Newtonsoft.Json;

    public static class EndpointRouteBuilderExtension
    {
        public static IEndpointRouteBuilder MapCustomHealthChecks(this IEndpointRouteBuilder builer)
        {
            builer.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (c, r) =>
                {
                    c.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject(r.Entries);
                    await c.Response.WriteAsync(result);
                }
            });
            return builer;
        }
    }
}