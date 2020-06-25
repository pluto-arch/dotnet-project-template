using Demo.Application.GrpcServices.GrpcCallers.Interfaces;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlogCore.Application;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Demo.Application.GrpcServices.GrpcCallers
{
    /// <summary>
    /// 调用外部grpc服务
    /// </summary>
    public class OrderGrpcService : IOrderGrpcService
    {
        private readonly GrpcUrlConfig _grpcUrlConfig;
        private readonly ILogger<OrderGrpcService> _logger;
        private readonly Metadata _grpcMetadata = null;

        public OrderGrpcService(
            HttpClient httpClient,
            ILogger<OrderGrpcService> logger,
            IOptions<GrpcUrlConfig> options, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _grpcUrlConfig = options.Value;
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var authorizationHeader = httpContextAccessor.HttpContext
                    .Request.Headers["Authorization"];
                _grpcMetadata = new Metadata();
                _grpcMetadata.Add("Authorization", authorizationHeader);
            }
        }

        public async Task<object> Get()
        {
            var result = await GrpcCallerService.CallServiceAsync(_grpcUrlConfig.OrderServiceAddress, async channel =>
            {
                var client = new OrderService.OrderServiceClient(channel);
                var response = await client.GetOrdersAsync(new Request { Id = 1231 }, _grpcMetadata);
                return response;
            });
            return result?.Name;
        }
    }
}