using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pluto.netcoreTemplate.API.Models;
using Pluto.netcoreTemplate.Application.Services.Interface;

namespace Pluto.netcoreTemplate.API.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;


        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// 获取所有订单  GET: api/Orders
        /// 使用订单微服务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse> Orders()
        {
            var orders =await _orderService.GetOrders();
            return ApiResponse.DefaultSuccess();
        }
    }
}