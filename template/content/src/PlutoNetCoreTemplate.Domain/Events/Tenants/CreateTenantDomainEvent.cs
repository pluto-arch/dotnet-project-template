namespace PlutoNetCoreTemplate.Domain.Events.Tenants
{
    using Aggregates.TenantAggregate;
    using MediatR;

    public class CreateTenantDomainEvent : INotification
    {
        public CreateTenantDomainEvent(Tenant tenant, bool isShareDatabase = true)
        {
            TenantId = tenant;
            IsShareDatabase = isShareDatabase;
        }
        public Tenant TenantId { get; set; }

        public bool IsShareDatabase { get; set; }

    }
}
