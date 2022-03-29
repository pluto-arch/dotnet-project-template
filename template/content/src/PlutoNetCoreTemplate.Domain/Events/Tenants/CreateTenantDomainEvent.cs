namespace PlutoNetCoreTemplate.Domain.Events.Tenants
{
    using MediatR;

    public class CreateTenantDomainEvent : INotification
    {
        public CreateTenantDomainEvent(string id, bool isShareDatabase, string databaseName = null)
        {
            TenantId = id;
            IsShareDatabase = isShareDatabase;
            var dd = databaseName;
        }
        public string TenantId { get; set; }

        public bool IsShareDatabase { get; set; }

        public string DataBaseName { get; set; }
    }
}
