using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using PlutoNetCoreTemplate.Controllers;
using PlutoNetCoreTemplate.Models;


namespace PlutoNetCoreTemplate.Test.ControllersTests
{
    public class ControllerTest:BaseTest
    {

        [Test]
        public async Task GET_api_Demo()
        {
            using (var scope = _Container.BeginLifetimeScope())
            {
                var _demoController = scope.Resolve<UserController>();
                var res= _demoController.Users();
                Assert.IsNotNull(res);
            }
        }
    }

}