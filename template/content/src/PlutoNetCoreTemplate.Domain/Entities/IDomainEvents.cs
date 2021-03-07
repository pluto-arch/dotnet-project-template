using System.Collections.Generic;

using MediatR;

namespace PlutoNetCoreTemplate.Domain.Entities
{
    public interface IDomainEvents
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }

        void AddDomainEvent(INotification eventItem);

        void RemoveDomainEvent(INotification eventItem);

        void ClearDomainEvents();
    }
}
