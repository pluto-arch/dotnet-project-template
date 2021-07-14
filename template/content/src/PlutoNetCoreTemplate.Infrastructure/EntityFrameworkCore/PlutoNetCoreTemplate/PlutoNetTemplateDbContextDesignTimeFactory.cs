namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Domain.Aggregates.TenantAggregate;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class PlutoNetTemplateDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PlutoNetTemplateDbContext>
    {
        public PlutoNetTemplateDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PlutoNetTemplateDbContext>();
            optionsBuilder.UseSqlServer(@"Server=127.0.0.1,1433;Database=Pnct_Default;User Id=sa;Password=970307lBX;Trusted_Connection = False;");
            return new PlutoNetTemplateDbContext(optionsBuilder.Options, new TenantProvider(null));
        }
    }
}