using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace PlutoNetCoreTemplate.Test.ApplicationTest
{
    using System;
    using System.Threading.Tasks;
    using System.Transactions;
    using Application.Permissions;
    using Domain.Aggregates.ProductAggregate;
    using Domain.Aggregates.TenantAggregate;
    using Domain.Repositories;
    using Domain.UnitOfWork;
    using Infrastructure.EntityFrameworkCore;

    public class ApplicationTest : BaseTest
    {
        private readonly string[] tenants = new[] {"T20210602000001","T20210602000002","T20210602000003"};


        [Test]
        public async Task TenantChangeForReadData()
        {
            var tenantProvider = serviceProvider.GetService<ITenantProvider>();
            if (tenantProvider is null)
            {
                Assert.Fail("no ITenantProvider found");
            }
            var currentTenant = serviceProvider.GetService<ICurrentTenant>();
            if (currentTenant is null)
            {
                Assert.Fail("no ICurrentTenant found");
            }
            var productRep = serviceProvider.GetService<IRepository<PlutoNetCoreTemplate.Domain.Aggregates.ProductAggregate.Product>>();
            if (productRep is null)
            {
                Assert.Fail("no Product repository found");
            }
            var t = await tenantProvider.InitTenant("T20210602000001");
            using (currentTenant.Change(t))
            {
                var res = await productRep.ToListAsync();
                Assert.IsNotNull(res);
                Assert.IsTrue(res.TrueForAll(x=>x.TenantId=="T20210602000001"));

                t = await tenantProvider.InitTenant("T20210602000002");
                using (currentTenant.Change(t))
                {
                    res = await productRep.ToListAsync();
                    Assert.IsNotNull(res);
                    Assert.IsTrue(res.TrueForAll(x=>x.TenantId=="T20210602000002"));
                }

                res = await productRep.ToListAsync();
                Assert.IsNotNull(res);
                Assert.IsTrue(res.TrueForAll(x=>x.TenantId=="T20210602000001"));
            }
        }


        [Test]
        public async Task TenantChangeForWriteData()
        {
            var tenantProvider = serviceProvider.GetService<ITenantProvider>();
            if (tenantProvider is null)
            {
                Assert.Fail("no ITenantProvider found");
            }
            var currentTenant = serviceProvider.GetService<ICurrentTenant>();
            if (currentTenant is null)
            {
                Assert.Fail("no ICurrentTenant found");
            }
            var productRep = serviceProvider.GetService<IRepository<PlutoNetCoreTemplate.Domain.Aggregates.ProductAggregate.Product>>();
            if (productRep is null)
            {
                Assert.Fail("no Product repository found");
            }

            var uow=serviceProvider.GetService<IUnitOfWork<DeviceCenterDbContext>>();
            if (uow is null)
            {
                Assert.Fail("no uow found");
            }
            var t = await tenantProvider.InitTenant("T20210602000001");
            using (currentTenant.Change(t))
            {
                var pro = new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = "单元测试数据22",
                    CreationTime = DateTimeOffset.Now
                };
                await productRep.InsertAsync(pro);

                t = await tenantProvider.InitTenant("T20210602000002");
                using (currentTenant.Change(t))
                {
                    await productRep.InsertAsync(pro);
                    await uow.SaveChangesAsync();
                }

                await productRep.InsertAsync(pro);
                await uow.SaveChangesAsync();
            }
        }


        [Test]
        public async Task TenantChangeWithTranScope()
        {
            var tenantProvider = serviceProvider.GetService<ITenantProvider>();
            if (tenantProvider is null)
            {
                Assert.Fail("no ITenantProvider found");
            }
            var currentTenant = serviceProvider.GetService<ICurrentTenant>();
            if (currentTenant is null)
            {
                Assert.Fail("no ICurrentTenant found");
            }
            var productRep = serviceProvider.GetService<IRepository<PlutoNetCoreTemplate.Domain.Aggregates.ProductAggregate.Product>>();
            if (productRep is null)
            {
                Assert.Fail("no Product repository found");
            }

            var uow=serviceProvider.GetService<IUnitOfWork<DeviceCenterDbContext>>();
            if (uow is null)
            {
                Assert.Fail("no uow found");
            }

            var t = await tenantProvider.InitTenant("T20210602000001");
            using (currentTenant.Change(t))
            {
                var pro = new Product
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = "单元测试数据22222",
                    CreationTime = DateTimeOffset.Now
                };
                await productRep.InsertAsync(pro);

                t = await tenantProvider.InitTenant("T20210602000002");
                using (currentTenant.Change(t))
                {
                    await productRep.InsertAsync(pro);
                    await uow.SaveChangesAsync();
                }

                await productRep.InsertAsync(pro);
                await uow.SaveChangesAsync();
            }
        }
    }
}
