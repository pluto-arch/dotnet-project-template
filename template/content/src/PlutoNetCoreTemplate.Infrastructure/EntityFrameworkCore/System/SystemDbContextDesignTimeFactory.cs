namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class SystemDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SystemDbContext>
    {
        public SystemDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SystemDbContext>();
            optionsBuilder.UseSqlServer(@"Server=127.0.0.1,1433;Database=Pnct_System;User Id=sa;Password=970307lBX;Trusted_Connection = False;");
            return new SystemDbContext(optionsBuilder.Options);
        }
    }
}