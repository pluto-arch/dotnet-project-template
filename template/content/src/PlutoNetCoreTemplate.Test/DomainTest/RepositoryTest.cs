using NUnit.Framework;

using PlutoNetCoreTemplate.Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;
using PlutoNetCoreTemplate.Domain.Aggregates.DemoTree;
using PlutoNetCoreTemplate.Domain.Aggregates.SystemAggregate;

namespace PlutoNetCoreTemplate.Test.DomainTest
{
    public class RepositoryTest : BaseTest
    {
        [Test]
        public void GetRepository()
        {
            var d = serviceProvider.GetService<ISystemBaseRepository<Folder>>();
            Assert.IsTrue(d != null);
        }
    }
}
