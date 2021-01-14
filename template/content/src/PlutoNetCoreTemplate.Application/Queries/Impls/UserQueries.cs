using PlutoData.Collections;
using PlutoData.Interface;
using PlutoNetCoreTemplate.Application.Dtos;
using PlutoNetCoreTemplate.Application.Queries.Interfaces;
using PlutoNetCoreTemplate.Domain.DomainModels.Account;
using PlutoNetCoreTemplate.Domain.IRepositories;
using PlutoNetCoreTemplate.Infrastructure;



namespace PlutoNetCoreTemplate.Application.Queries.Impls
{
    /// <summary>
    /// 仅仅查询-默认不启用追踪
    /// 方法指定 disableTracking:true
    /// </summary>
    public class UserQueries: IUserQueries
    {

        private readonly IRepository<UserEntity> _userRepository;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UserQueries(IUnitOfWork<EfCoreDbContext> unitOfWork)
        {
            _userRepository = unitOfWork.GetRepository<IUserRepository>();
        }


        /// <inheritdoc />
        public IPagedList<UserItemDto> GetUsers()
        {
            var pageList= _userRepository.GetPagedList<UserItemDto>(x => new UserItemDto{UserName=x.UserName,Email=x.Email},pageIndex:1,pageSize:20);
            return pageList;
        }

        /// <inheritdoc />
        public object GetUser(object key)
        {
            return _userRepository.Find(key);
        }
    }
}