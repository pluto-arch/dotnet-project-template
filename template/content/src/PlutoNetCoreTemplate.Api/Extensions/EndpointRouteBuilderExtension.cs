namespace PlutoNetCoreTemplate.Api.Extensions
{
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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