using System.Net;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlutoNetCoreTemplate.Models;

namespace PlutoNetCoreTemplate.Middlewares
{
    public class CustomRateLimitMiddleware: RateLimitMiddleware<IpRateLimitProcessor>
    {
        private readonly ILogger<IpRateLimitMiddleware> _logger;
        public CustomRateLimitMiddleware(
            RequestDelegate next,
            IOptions<IpRateLimitOptions> options,
            IRateLimitCounterStore counterStore,
            IIpPolicyStore policyStore,
            IRateLimitConfiguration config,
            ILogger<IpRateLimitMiddleware> logger) : base(next, (RateLimitOptions)options?.Value, new IpRateLimitProcessor(options?.Value, counterStore, policyStore, config), config)
        {
            _logger = logger;
        }

        protected override void LogBlockedRequest(HttpContext httpContext, ClientRequestIdentity identity, RateLimitCounter counter,
            RateLimitRule rule)
        {
            var message = "来自{@ip}的请求 {@httpMethod}:{@requestPath} 过于频繁，被终止。当前速率限制规则：{@limit}/{@period}";
            _logger.LogInformation(message, identity.ClientIp, (object)identity.HttpVerb, (object)identity.Path, (object)rule.Limit, (object)rule.Period);
        }


        public override async Task ReturnQuotaExceededResponse(HttpContext httpContext, RateLimitRule rule, string retryAfter)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            httpContext.Response.ContentType = "application/json;charset=utf-8";
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(ApiResponse.Error("请求太频繁，请稍后再试")));
        }
    }
}