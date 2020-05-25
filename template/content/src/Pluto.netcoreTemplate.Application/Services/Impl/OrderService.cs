using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pluto.netcoreTemplate.Application.ResourceModels.Order;
using Pluto.netcoreTemplate.Application.Services.Interface;
using Pluto.netcoreTemplate.Application.Services.Settings;


namespace Pluto.netcoreTemplate.Application.Services.Impl
{
    /// <summary>
    /// demo 访问另外的为服务
    /// </summary>
    /// <remarks>
    /// 使用http方式
    /// </remarks>
    public class OrderService:IOrderService
    {
        /*
         * 若使用Http方式，需要封装HTTP GET/POST等操作，以及证书等请封装到基础设施中
         */

        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderService> _logger;
        private readonly IOptions<OrderSetting> _settings;
        private readonly string _remoteServiceBaseUrl;
        public OrderService(
            HttpClient httpClient, 
            ILogger<OrderService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _remoteServiceBaseUrl = httpClient.BaseAddress.AbsoluteUri;
        }


        /// <inheritdoc />
        public async Task<GetOrdersResponse> GetOrders(int pageIndex = 1, int pageSize = 20)
        {
            var uri = API.Order.GetOrder($"{_remoteServiceBaseUrl}/Orders", pageIndex,pageSize);
            var responseString = await _httpClient.GetStringAsync(uri);
            var catalog = JsonConvert.DeserializeObject<GetOrdersResponse>(responseString);
            return catalog;
        }
    }
}