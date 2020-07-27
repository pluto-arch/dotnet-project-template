using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Serilog;


namespace Demo.Application.GrpcServices
{
    public  class GrpcCallerService
    {
        private readonly ILogger<GrpcCallerService> _logger;
        public GrpcCallerService(ILogger<GrpcCallerService> logger)
        {
            _logger = logger;
        }


        public  async Task<TResponse> CallServiceAsync<TResponse>(string urlGrpc,
           Func<GrpcChannel, Task<TResponse>> func)
        {
            //若要使用 .NET Core 客户端调用不安全的 gRPC 服务，需要其他配置。 gRPC 客户端必须将 System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport 开关设置为 true 并在服务器地址中使用 http：
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);


            var channel = GrpcChannel.ForAddress(urlGrpc);
            _logger.LogInformation("创建GRPC客户端 urlGrpc ={@urlGrpc}, BaseAddress={@BaseAddress} ", urlGrpc, channel.Target);
            try
            {
                return await func(channel);
            }
            catch (RpcException e)
            {
                _logger.LogError(e, "GRPC调用异常: {@BaseAddress} - {Message}", channel.Target, e.Message);
                return default;
            }
            finally
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }
        }

        public  async Task CallService(string urlGrpc, Func<GrpcChannel, Task> func)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            var channel = GrpcChannel.ForAddress(urlGrpc);
            _logger.LogInformation("创建GRPC客户端 urlGrpc ={@urlGrpc}, BaseAddress={@BaseAddress} ", urlGrpc, channel.Target);
            try
            {
                await func(channel);
            }
            catch (RpcException e)
            {
                _logger.LogError(e, "GRPC调用异常: {@BaseAddress} - {Message}", channel.Target, e.Message);
            }
            finally
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }
        }
    }
}