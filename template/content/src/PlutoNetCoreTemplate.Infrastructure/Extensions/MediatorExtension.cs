
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PlutoNetCoreTemplate.Domain.Entities;

namespace PlutoNetCoreTemplate.Infrastructure.Extensions
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, PlutoNetTemplateDbContext ctx, CancellationToken cancellationToken = default)
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