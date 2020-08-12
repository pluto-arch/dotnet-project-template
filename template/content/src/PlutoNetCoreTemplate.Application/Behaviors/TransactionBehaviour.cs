using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using PlutoNetCoreTemplate.Application.Attributes;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoNetCoreTemplate.Infrastructure.Extensions;
using PlutoNetCoreTemplate.Infrastructure.Providers;

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PlutoData.Interface;

namespace PlutoNetCoreTemplate.Application.Behaviors
{
    /// <summary>
    /// 涉及事务的所有操作
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly EventIdProvider _eventIdProvider;

        private readonly IUnitOfWork<EfCoreDbContext> _unitOfWork;

        public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger, EventIdProvider eventIdProvider, IUnitOfWork<EfCoreDbContext> unitOfWork)
        {
            _logger = logger;
            _eventIdProvider = eventIdProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();
            var type = request.GetType();
            var tranAttr = type.GetCustomAttribute(typeof(CommandTransactionAttribute), true) as CommandTransactionAttribute;
            if (tranAttr == null)
            {
                return await next();
            }
            if (_unitOfWork.HasActiveTransaction)
            {
                return await next();
            }
            var strategy = _unitOfWork.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                Guid transactionId;
                using (var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken))
                {
                    transactionId = transaction.TransactionId;
                    _logger.LogInformation(_eventIdProvider.EventId, "Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);
                    response = await next();
                    await _unitOfWork.CommitTransactionAsync(transaction,cancellationToken);
                    _logger.LogInformation(_eventIdProvider.EventId, "Finish transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);
                }
                // TODO 事务执行完毕后 通过 事件总线 发布，从而处理其余业务 
                //await _orderingIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
            });
            return response;
        }
    }
}