using System;

namespace EventBus
{
    /// <summary>
    /// 订阅信息
    /// </summary>
    public class SubscriptionInfo
    {
        /// <summary>
        /// 是否是动态事件
        /// </summary>
        public bool IsDynamic { get; }

        /// <summary>
        /// 对应处理程序类型
        /// </summary>
        public Type HandlerType { get; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        private SubscriptionInfo(Type handlerType)
        {
            HandlerType = handlerType;
            Enabled = true;
        }

        private SubscriptionInfo(bool isDynamic, Type handlerType)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
            Enabled = true;
        }

        public static SubscriptionInfo Typed(Type handlerType)
        {
            return new SubscriptionInfo(handlerType);
        }


        public static SubscriptionInfo Dynamic(Type handlerType)
        {
            return new SubscriptionInfo(true, handlerType);
        }
    }
}
