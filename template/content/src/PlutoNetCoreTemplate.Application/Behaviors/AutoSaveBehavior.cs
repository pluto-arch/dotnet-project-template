using System.Linq;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlutoNetCoreTemplate.Application.Attributes;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoData.Uows;

namespace PlutoNetCoreTemplate.Application.Behaviors
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class AutoSaveBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEfUnitOfWork<EfCoreDbContext> _uow;
        private readonly ILogger<AutoSaveBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="logger"></param>
        public AutoSaveBehavior(IEfUnitOfWork<EfCoreDbContext> uow,ILogger<AutoSaveBehavior<TRequest, TResponse>> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("执行{@command}：{@Command}",request.GetType().Name, request);
            TResponse response = default;
            var type = request.GetType();
            if (type.GetCustomAttribute(typeof(DisableAutoSaveChangeAttribute), true) is DisableAutoSaveChangeAttribute attr)
            {
                return await next();
            }
            else
            {
                response = await next();
            }
            if (_uow.DbContext.ChangeTracker.Entries().Any(x => x.State != EntityState.Unchanged))
            {
                await _uow.SaveChangesAsync(cancellationToken);
            }
            _logger.LogInformation("执行{@command} result: {@Response}",request.GetType().Name, response);
            return response;
        }
    }
}