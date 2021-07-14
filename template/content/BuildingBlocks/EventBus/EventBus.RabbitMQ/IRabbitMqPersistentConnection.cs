using RabbitMQ.Client;

using System;

namespace EventBus.RabbitMQ
{
    /// <summary>
    /// rabbit mq连接
    /// </summary>
    public interface IRabbitMqPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}