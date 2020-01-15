using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Pluto.netcoreTemplate.Application.Attributes;
using Pluto.netcoreTemplate.Infrastructure;
using Pluto.netcoreTemplate.Infrastructure.Extensions;
using Pluto.netcoreTemplate.Infrastructure.Providers;

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Pluto.netcoreTemplate.Application.Behaviors
{
    /// <summary>
    /// 涉及事务的所有操作
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly PlutonetcoreTemplateDbContext _dbContext;
        private readonly EventIdProvider _eventIdProvider;
        public TransactionBehaviour(PlutonetcoreTemplateDbContext dbContext, ILogger<TransactionBehaviour<TRequest, TResponse>> logger, EventIdProvider eventIdProvider)
        {
            _dbContext = dbContext;
            _logger = logger;
            _eventIdProvider = eventIdProvider;
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
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;
                    using (var transaction = await _dbContext.BeginTransactionAsync())
                    {
                        _logger.LogInformation(_eventIdProvider.EventId, "----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                        response = await next();

                        _logger.LogInformation(_eventIdProvider.EventId, "----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                        await _dbContext.CommitTransactionAsync(transaction);
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