namespace PlutoNetCoreTemplate.Infrastructure.DapperCore
{
    using System;
    using System.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using PlutoData;
    using PlutoData.Enums;

    public class PlutoNetCoreDapperDbContext:DapperDbContext
    {
        /// <inheritdoc />
        public PlutoNetCoreDapperDbContext(IServiceProvider service, DapperDbContextOption<PlutoNetCoreDapperDbContext> options) : base(service, options)
        {
        }
    }
}