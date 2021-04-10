namespace PlutoNetCoreTemplate.Application.AppServices
{
    using System.Threading.Tasks;
    using Dapper;
    using Domain.Aggregates.System;
    using Domain.SeedWork;
    using Domain.Services.Account;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using PlutoData.Collections;
    using PlutoData.Specifications;

    public class SystemAppService:ISystemAppService
    {

        private readonly ISystemDomainService _accountDomainService;
        private readonly EfCoreDbContext ddd;
        private readonly IPlutoNetCoreTemplateDapperRepository<UserEntity> _userDapperRep;

        public SystemAppService(ISystemDomainService accountDomainService,EfCoreDbContext db,IPlutoNetCoreTemplateDapperRepository<UserEntity> rrr)
        {
            _accountDomainService = accountDomainService;
            ddd = db;
            _userDapperRep = rrr;
        }

        /// <inheritdoc />
        public IPagedList<UserEntity> GetPageList(int pageNo, int pageSize)
        {
            var pageList= _accountDomainService.GetUserPageList(new UserSpecification(), 1,20);
            return pageList;
        }

        /// <inheritdoc />
        public async Task<UserEntity> GetUser(object key)
        {
            return await _accountDomainService.Find(key);
        }


        public async Task<int> CreateUserAsync(string name,string email)
        {
            var conn=this.ddd.Database.GetDbConnection();
            var entity = new UserEntity
            {
                UserName = name,
                Email = email,
            };

            await _userDapperRep.ExecuteAsync(async conn =>
            {
                await conn.InsertAsync(entity);
            });

            var res = await _accountDomainService.InsertAsync(entity);
            return res ?? 0;
        }
    }
}