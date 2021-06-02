namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.Aggregates.TenantAggregate;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class DbContextDesignTimeFactory: IDesignTimeDbContextFactory<PlutoNetTemplateDbContext>
    {
        public PlutoNetTemplateDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PlutoNetTemplateDbContext>();
            optionsBuilder.UseSqlServer(@"Server=127.0.0.1,1433;Database=EfCore2;User Id=sa;Password=970307Lbx$;Trusted_Connection = False;");
            return new PlutoNetTemplateDbContext(optionsBuilder.Options,new TenantProvider(null));
        }
    }
}