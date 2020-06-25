using System;


namespace Demo.Application.GrpcServices
{
    public class GrpcUrlConfig
    {
        public const string ConfigKey = "Grpc:Urls";
        public string OrderServiceAddress { get; set; }
    }
}