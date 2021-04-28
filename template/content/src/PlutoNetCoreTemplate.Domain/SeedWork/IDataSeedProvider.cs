using System;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    public interface IDataSeedProvider
    {
        /// <summary>
        /// 种子数据的初始化顺序
        /// </summary>
        int Sorts { get; }
        Task SeedAsync(IServiceProvider serviceProvider);
    }
}
