namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class TenantDbContextDesignTimeFactory:IDesignTimeDbContextFactory<TenantDbContext>
    {
        public TenantDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            optionsBuilder.UseSqlServer(@"Server=127.0.0.1,1433;Database=Pnct_Tenant;User Id=sa;Password=970307lBX;Trusted_Connection = False;");
            return new TenantDbContext(optionsBuilder.Options);
        }
    }
}