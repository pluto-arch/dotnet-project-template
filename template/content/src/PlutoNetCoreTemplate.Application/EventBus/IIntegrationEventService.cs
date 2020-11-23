using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Application.EventBus.IntegrationEvents
{
    /// <summary>
    /// 集成事件服务
    /// </summary>
    public interface IIntegrationEventService
    {
        /// <summary>
        /// 向外部分布式系统广播事件
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task PublishEventsThroughEventBusAsync(Guid transactionId);

        /// <summary>
        /// 领域事件触发后保存事件信息
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        Task AddAndSaveEventAsync(object evt);
    }
}
