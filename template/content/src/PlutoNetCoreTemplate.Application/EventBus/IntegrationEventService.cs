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
    public class IntegrationEventService : IIntegrationEventService
    {
        /// <summary>
        /// 领域事件触发后保存事件信息
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public async Task AddAndSaveEventAsync(object evt)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 将保存的事件信息广播到其余分布式系统
        /// <para>
        /// 可使用事件总线
        /// </para>
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            await Task.CompletedTask;
        }
    }
}
