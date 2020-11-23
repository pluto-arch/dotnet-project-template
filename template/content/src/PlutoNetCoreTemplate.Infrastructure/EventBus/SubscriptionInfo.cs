using System;

namespace PlutoNetCoreTemplate.Infrastructure.EventBus
{
    /// <summary>
    /// 订阅信息
    /// </summary>
    public class SubscriptionInfo
    {
        /// <summary>
        /// 处理程序类型
        /// </summary>
        public Type HandlerType { get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        private SubscriptionInfo(Type handlerType,string description)
        {
            HandlerType = handlerType;
            Description=description;
        }

        public static SubscriptionInfo Typed(Type handlerType,string description)
        {
            return new SubscriptionInfo(handlerType,description);
        }
    }
}