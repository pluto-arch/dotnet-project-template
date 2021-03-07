using System;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    public interface IDataSeedProvider
    {
        Task SeedAsync(IServiceProvider serviceProvider);
    }
}
