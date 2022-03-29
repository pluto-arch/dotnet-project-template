using Microsoft.AspNetCore.Http;

namespace PlutoNetCoreTemplate.Api.Infrastructure.UnitOfWork
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using PlutoNetCoreTemplate.Domain.UnitOfWork;

    using System.Threading.Tasks;

    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;

        public UnitOfWorkMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var uowOptions = context.RequestServices.GetService<IOptions<UnitOfWorkCollectionOptions>>()?.Value;
            if (uowOptions is not null && uowOptions?.DbContexts is { Count: > 0 })
            {
                foreach (var item in uowOptions?.DbContexts)
                {
                    var uow = context.RequestServices.GetService(item.Value) as IUnitOfWork;
                    if (uow is null)
                    {
                        continue;
                    }
                    await uow?.SaveChangesAsync(context.RequestAborted);
                }
            }

            await _next(context);
        }
    }
}