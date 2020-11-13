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

        private readonly IUnitOfWork<EfCoreDbContext> _unitOfWork;

        public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger, IUnitOfWork<EfCoreDbContext> unitOfWork)
        {
            _logger = logger;
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
                    _logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);
                    response = await next();
                    await _unitOfWork.CommitTransactionAsync(transaction,cancellationToken);
                    _logger.LogInformation("Finish transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);
                }
                // TODO 事务执行完毕后 通过 事件总线 发布，从而处理其余业务 
                // 集成事件
                // 过程说明：
                /*
                 * 集成事件与领域事件是一起相互作用的，
                 * 领域事件激发时，对应的handler将此次事件的信息保存到集成事件的日志表中，保存这个操作，使用到了对应发生事件的领域实体所在的上下文的事务对象，以保证强一致性(在领域事件的handler中，通过_unitOfWork.GetCurrentTransaction();来获取实体db上下文的事务对象，然后保存(注意此处的transactionId)。)
                 * 然后事务提交完成后，实体的更改和事件日志都会被记录到数据库。使用同一个事务保证了一致性
                 * 然后通过集成时间服务，根据次事务id查询出需要激发的领域事件数据，然后遍历操作：
                 * 1、标记为处理中
                 * 2、通过事件总线(可以支持MQ等消息队列或者其他组件进行发布)
                 * 3、标记为处理完成、遇到异常标记为失败(此处可增加policy的策略重试功能，但是会影响本次操作的响应时间)，由后台任务定时重试这些失败的事件。确保最终一致性。但是消费者端的一致性无法保证(需要其他策略机行处理)。极大可能的保证最终一致性。
                 * 
                 * 如果此次操作没有涉及通知其他系统，可不使用事务，对应的command对象上不需要加CommandTransactionAttribute特性(反射也会影响性能)。
                 */
                //await _orderingIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
            });
            return response;
        }
    }
}