using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using PlutoNetCoreTemplate.Application.Queries.Interfaces;

namespace PlutoNetCoreTemplate.Test.ApplicationTest
{
    public class ApplicationTest : BaseTest
    {
        [Test]
        public void GET_Users()
        {
            var userQueries = serviceProvider.GetService<IUserQueries>();
            var res = userQueries.GetUsers();
            Assert.IsNotNull(res);
        }
    }
}
