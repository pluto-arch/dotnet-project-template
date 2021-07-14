namespace PlutoNetCoreTemplate.Application.DomainEventHandler
{
    using Domain.Events.Products;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using System.Threading;
    using System.Threading.Tasks;

    public class CreateProductDomainEventHandler : INotificationHandler<CreateProductDomainEvent>
    {
        private readonly ILogger<CreateProductDomainEventHandler> _logger;
        public CreateProductDomainEventHandler(ILogger<CreateProductDomainEventHandler> logger)
        {
            _logger = logger;
        }


        /// <inheritdoc />
        public async Task Handle(CreateProductDomainEvent notification, CancellationToken cancellationToken)
        {
            await Task.Delay(1);
            _logger.LogInformation("处理：{@event} , 参数：{@param}", nameof(CreateProductDomainEvent), notification);
        }
    }
}