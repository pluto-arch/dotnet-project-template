using Demo.Application.GrpcServices.GrpcCallers.Interfaces;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlogCore.Application;
using Demo.Infrastructure.Providers;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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
        private readonly EventIdProvider _eventIdProvider;
        private readonly GrpcCallerService _callerService;
        public OrderGrpcService(
            HttpClient httpClient,
            ILogger<OrderGrpcService> logger,
            IOptions<GrpcUrlConfig> options,
            IHttpContextAccessor httpContextAccessor,
            EventIdProvider eventIdProvider, 
            GrpcCallerService callerService)
        {
            _logger = logger;
            _eventIdProvider = eventIdProvider;
            _callerService = callerService;
            _grpcUrlConfig = options.Value;
            _grpcMetadata = new Metadata();
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var authorizationHeader = httpContextAccessor.HttpContext
                    .Request.Headers["Authorization"];
                _grpcMetadata.Add("Authorization", authorizationHeader);
            }
        }

        public async Task<object> Get()
        {
            var result = await _callerService.CallServiceAsync(_grpcUrlConfig.OrderServiceAddress, async channel =>
            {
                var client = new OrderService.OrderServiceClient(channel);
                var request = new Request { Id = 1231 };
                _logger.LogInformation(_eventIdProvider.EventId, "开始调用OrderGrpc(GetOrdersAsync)，请求参数：{@request}", request);
                _grpcMetadata.Add(new Metadata.Entry("EventId", JsonConvert.SerializeObject(_eventIdProvider.EventId)));
                var response = await client.GetOrdersAsync(request, _grpcMetadata);
                return response;
            });
            return result?.Name;
        }
    }
}