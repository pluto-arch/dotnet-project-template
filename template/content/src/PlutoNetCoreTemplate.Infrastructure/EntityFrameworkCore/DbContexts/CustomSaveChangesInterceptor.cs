﻿using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
using PlutoNetCoreTemplate.Domain.Entities;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.SeedWork;

    public class CustomSaveChangesInterceptor : SaveChangesInterceptor
    {
        //private readonly IMediator _mediator;

        private readonly IDomainEventDispatcher _dispatcher;


        public CustomSaveChangesInterceptor(IDomainEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }


        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            MultiTenancyTracking(eventData.Context);
            SoftDeleteTracking(eventData.Context);
            DispatchDomainEventsAsync(eventData.Context).Wait();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            MultiTenancyTracking(eventData.Context);
            SoftDeleteTracking(eventData.Context);
            await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void SoftDeleteTracking(DbContext dbContext)
        {
            var deletedEntries = dbContext.ChangeTracker.Entries().Where(entry => entry.State == EntityState.Deleted && entry.Entity is ISoftDelete);
            deletedEntries?.ToList().ForEach(entityEntry =>
            {
                entityEntry.Reload();
                entityEntry.State = EntityState.Modified;
                ((ISoftDelete)entityEntry.Entity).Deleted = true;
            });
        }

        private static void MultiTenancyTracking(DbContext dbContext)
        {
            var tenantedEntries = dbContext.ChangeTracker.Entries<IMultiTenant>().Where(entry => entry.State == EntityState.Added);
            var currentTenant = dbContext.GetService<ICurrentTenant>();
            tenantedEntries?.ToList().ForEach(entityEntry =>
            {
                entityEntry.Entity.TenantId ??= currentTenant.Id;
            });
        }

        private async Task DispatchDomainEventsAsync(DbContext dbContext, CancellationToken cancellationToken = default)
        {
            var domainEntities = dbContext.ChangeTracker.Entries<IDomainEvents>().Select(e => e.Entity);
            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents).ToList();
            domainEntities.ToList().ForEach(entity => entity.ClearDomainEvents());
            foreach (var domainEvent in domainEvents)
            {
                await _dispatcher.Dispatch(domainEvent);
            }
        }
    }
}