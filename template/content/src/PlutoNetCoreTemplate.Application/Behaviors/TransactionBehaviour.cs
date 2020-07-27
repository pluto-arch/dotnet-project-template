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

        private readonly IUnitOfWork<PlutoNetCoreTemplateDbContext> _unitOfWork;

        public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger, EventIdProvider eventIdProvider, IUnitOfWork<PlutoNetCoreTemplateDbContext> unitOfWork)
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
            try
            {
                if (_unitOfWork.HasActiveTransaction)
                {
                    return await next();
                }
                var strategy = _unitOfWork.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;
                    using (var transaction = await _unitOfWork.BeginTransactionAsync())
                    {
                        _logger.LogInformation(_eventIdProvider.EventId, "----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                        response = await next();

                        _logger.LogInformation(_eventIdProvider.EventId, "----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                        await _unitOfWork.CommitTransactionAsync(transaction);
                        _logger.LogInformation(_eventIdProvider.EventId, "----- Finish transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);
                        transactionId = transaction.TransactionId;
                    }
                    // TODO 事务执行完毕后 通过 事件总线 发布，从而处理其余业务 
                    //await _orderingIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
                });
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(_eventIdProvider.EventId, ex, "为命令：{CommandName} ({@Command})。进行事务处理异常", typeName, request);
                throw;
            }
        }
    }
}