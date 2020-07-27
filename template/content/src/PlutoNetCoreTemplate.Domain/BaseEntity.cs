using MediatR;

using PlutoNetCoreTemplate.Domain.SeedWork;

using System;
using System.Collections.Generic;

namespace PlutoNetCoreTemplate.Domain
{

    public class Entity
    {
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();
        internal void AddDomainEvent(INotification eventItem)
        {
            _domainEvents ??= new List<INotification>();
            _domainEvents.Add(eventItem);
        }
        internal void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

    }

    public class BaseEntity<TKey> : Entity, IAggregateRoot
    {
        public virtual TKey Id { get; set; }


        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ^ 31;
        }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BaseEntity<TKey>))
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;
            return false;
        }

        public static bool operator ==(BaseEntity<TKey> left, BaseEntity<TKey> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(BaseEntity<TKey> left, BaseEntity<TKey> right)
        {
            return !(left == right);
        }

    }
}