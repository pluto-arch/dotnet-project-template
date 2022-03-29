namespace PlutoNetCoreTemplate.Api.Filters
{
    using Domain.UnitOfWork;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class UnitOfWorkFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            var http = context.HttpContext;
            var uowOptions = http.RequestServices.GetService<IOptions<UnitOfWorkCollectionOptions>>()?.Value;
            if (uowOptions is not null && uowOptions?.DbContexts is { Count: > 0 })
            {
                foreach (var item in uowOptions?.DbContexts)
                {
                    var uow = http.RequestServices.GetService(item.Value) as IUnitOfWork;
                    uow?.SaveChangesAsync(http.RequestAborted);
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}