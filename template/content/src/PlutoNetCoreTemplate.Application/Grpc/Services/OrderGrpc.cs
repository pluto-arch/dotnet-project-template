#if (Grpc)
using System;

namespace PlutoNetCoreTemplate.Application.Grpc.Services
{
    
    
    
    using System.Threading.Tasks;
    using global::Grpc.Core;

    public class OrderGrpc:OrderingGrpc.OrderingGrpcBase
    {
        /// <inheritdoc />
        public override Task<Response> Service_A(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response{UserName = "123123",OrderId = "123123"});
        }
    }
}
#endif