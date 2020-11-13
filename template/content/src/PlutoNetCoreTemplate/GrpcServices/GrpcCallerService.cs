using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Serilog;


namespace PlutoNetCoreTemplate.GrpcServices
{
    public static class GrpcCallerService
    {
        public static async Task<TResponse> CallServiceAsync<TResponse>(string urlGrpc,
           Func<GrpcChannel, Task<TResponse>> func)
        {
            //若要使用 .NET Core 客户端调用不安全的 gRPC 服务，需要其他配置。 gRPC 客户端必须将 System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport 开关设置为 true 并在服务器地址中使用 http：
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);


            var channel = GrpcChannel.ForAddress(urlGrpc);
            Log.Information("Creating grpc client base address urlGrpc ={@urlGrpc}, BaseAddress={@BaseAddress} ", urlGrpc, channel.Target);
            try
            {
                return await func(channel);
            }
            catch (RpcException e)
            {
                Log.Error(e, "Error calling grpc: {@BaseAddress} - {Message}", channel.Target, e.Message);
                return default;
            }
            finally
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }
        }

        public static TResponse CallService<TResponse>(string urlGrpc, Func<GrpcChannel, TResponse> func)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            var channel = GrpcChannel.ForAddress(urlGrpc);
            try
            {
                return func(channel);
            }
            catch (RpcException e)
            {
                Log.Error(e, "Error calling grpc: {@BaseAddress} - {Message}", channel.Target, e.Message);
                return default;
            }
            finally
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }
        }
    }
}