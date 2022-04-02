namespace PlutoNetCoreTemplate.Infrastructure.Providers
{
    using System.Collections.Generic;
    using MediatR;

    using System.Threading;
    using System.Threading.Tasks;


    public class NullMediatorProvider
    {
        public static IMediator GetNullMediator() => new NullMediator();
    }


    public class NullMediator : IMediator
    {
        /// <inheritdoc />
        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<TResponse>(default!);
        }

        /// <inheritdoc />
        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<object>(default);
        }

        /// <inheritdoc />
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
        {
            return default;
        }

        /// <inheritdoc />
        public IAsyncEnumerable<object> CreateStream(object request, CancellationToken cancellationToken = new CancellationToken())
        {
            return default;
        }
    }
}