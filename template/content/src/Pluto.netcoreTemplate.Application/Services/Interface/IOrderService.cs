using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pluto.netcoreTemplate.Application.ResourceModels.Order;

namespace Pluto.netcoreTemplate.Application.Services.Interface
{
    /// <summary>
    /// order 服务client接口顶级
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<GetOrdersResponse> GetOrders(int pageIndex = 1, int pageSize = 20);
    }
}