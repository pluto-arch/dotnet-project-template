namespace PlutoNetCoreTemplate.Test.ApplicationTest
{
    using PlutoNetCoreTemplate.Application.Queries.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using PlutoData.Interface;
    using PlutoNetCoreTemplate.Domain.DomainModels.Account;

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
