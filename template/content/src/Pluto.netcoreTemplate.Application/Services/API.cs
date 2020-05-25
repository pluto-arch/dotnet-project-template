using System;


namespace Pluto.netcoreTemplate.Application.Services
{
    public static class API
    {
        public static class Order
        {
            public static string GetOrder(string baseUri, int pageIndex=1,int pageSize=20) => $"{baseUri}?pageIndex={pageIndex}&pageSize={pageSize}";
        }
    }
}