namespace PlutoNetCoreTemplate.Test.DomainTest
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
    using PlutoData.Uows;
    using PlutoNetCoreTemplate.Infrastructure;
    using PlutoData;
    using PlutoNetCoreTemplate.Infrastructure.Repositories.Dapper;

    public class RepositoryTest:BaseTest
    {
        [Test]
        public void DapperRep()
        {
            var userDapperRep = serviceProvider.GetService<IDapperRepository<UserEntity>>();
            Assert.IsNotNull(userDapperRep);
            var res=userDapperRep.GetPageList(null,1,20);
            Assert.IsNotNull(res);

            var dapper_uow=serviceProvider.GetService<IDapperUnitOfWork<DapperDbContext>>();
            Assert.IsNotNull(dapper_uow);

            var user_dapper_rep_from_dapper_uow=dapper_uow.GetRepository<IUserDapperRepository>();
            Assert.IsNotNull(user_dapper_rep_from_dapper_uow);
            Assert.IsTrue(user_dapper_rep_from_dapper_uow is IDapperRepository<UserEntity>);

            var ef_uow=serviceProvider.GetService<IEfUnitOfWork<EfCoreDbContext>>();
            var user_dapper_rep_from_efuow=ef_uow.GetDapperRepository<IDapperRepository<UserEntity>,DapperDbContext>();
            Assert.IsNotNull(user_dapper_rep_from_efuow);
            Assert.IsTrue(user_dapper_rep_from_efuow is IDapperRepository<UserEntity>);
            Assert.AreEqual(user_dapper_rep_from_efuow.GetHashCode(),userDapperRep.GetHashCode());
        }
    }
}
