using System;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Pluto.netcoreTemplate.API.Controllers;


namespace Pluto.netcoreTemplate.Test.ApiTest
{
    public class DemoControllerTest:BaseTest
    {

        [Test]
        public void GET_api_Demo()
        {
            using (var scope = _Container.BeginLifetimeScope())
            {
                var _demoController = scope.Resolve<UserController>();
                var res= _demoController.Users();
                Assert.IsTrue(res.Successed);
            }
        }
    }

}