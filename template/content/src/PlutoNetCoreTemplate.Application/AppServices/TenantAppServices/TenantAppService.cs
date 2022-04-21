namespace PlutoNetCoreTemplate.Application.AppServices.TenantAppServices
{
    using Domain.Aggregates.TenantAggregate;
    using Domain.Events.Tenants;
    using Domain.Repositories;
    using Models.TenantModels;

    public class TenantAppService : ITenantAppService
    {
        private readonly IRepository<Tenant> _tenants;
        private readonly IMapper _mapper;
        readonly Random r = new();

        public TenantAppService(IRepository<Tenant> tenants, IMapper mapper)
        {
            _tenants = tenants;
            _mapper = mapper;
        }

        public async Task<List<TenantDto>> GetListAsync()
        {
            var entities = await _tenants.AsNoTracking().ToListAsync(); ;
            return _mapper.Map<List<TenantDto>>(entities);
        }


        public async Task<TenantDto> CreateAsync()
        {
            var now = DateTime.Now;
            var id = $@"T{now.Ticks}{r.Next(10000, 99999)}";
            var name = $@"租户{now.Ticks}";
            var connStr = $@"Server=127.0.0.1,1433;Database=Pnct_{id};User Id=sa;Password=970307lBX;Trusted_Connection = False;";
            var entity = new Tenant
            {
                Id = id,
                Name = name,
            };
            if (!string.IsNullOrEmpty(connStr))
            {
                entity.AddConnectionStrings("Default", connStr);
            }
            entity.AddDomainEvent(new CreateTenantDomainEvent(entity, !string.IsNullOrEmpty(connStr)));
            entity = await _tenants.InsertAsync(entity);
            return _mapper.Map<TenantDto>(entity);
        }
    }
}