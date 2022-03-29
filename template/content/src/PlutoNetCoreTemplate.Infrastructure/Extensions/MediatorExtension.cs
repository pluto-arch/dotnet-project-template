
using MediatR;

using PlutoNetCoreTemplate.Domain.Entities;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Infrastructure.Extensions
{
    using EntityFrameworkCore;

    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, DeviceCenterDbContext ctx, CancellationToken cancellationToken = default)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<BaseEntity>()
                .OfType<IDomainEvents>();
            var domainEvents = domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();
            domainEntities
                .ToList()
                .ForEach(entity => entity.ClearDomainEvents());
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent, cancellationToken);
            }
        }
    }
}