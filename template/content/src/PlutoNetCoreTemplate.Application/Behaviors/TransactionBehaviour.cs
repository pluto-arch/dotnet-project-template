using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlutoNetCoreTemplate.Application.Attributes;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoNetCoreTemplate.Infrastructure.Extensions;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PlutoData.Uows;

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

        private readonly IEfUnitOfWork<EfCoreDbContext> _unitOfWork;

        public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger, IEfUnitOfWork<EfCoreDbContext> unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();
            var type = request.GetType();
            if (!(type.GetCustomAttribute(typeof(CommandTransactionAttribute), true) is CommandTransactionAttribute tranAttr))
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
                await using (var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken))
                {
                    transactionId = transaction.TransactionId;
                    _logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);
                    response = await next();
                    await _unitOfWork.CommitTransactionAsync(transaction,cancellationToken);
                    _logger.LogInformation("Finish transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);
                }
                // TODO 事务执行完毕后 通过 事件总线 发布，从而处理其余业务 
            });
            return response;
        }
    }
}