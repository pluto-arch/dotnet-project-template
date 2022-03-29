using PlutoNetCoreTemplate.Application.Models.Generics;
using PlutoNetCoreTemplate.Domain.Entities;

namespace PlutoNetCoreTemplate.Application.AppServices.Generics
{
    using AutoMapper;

    using Domain.Repositories;

    using System.Threading.Tasks;


    /// <summary>
    /// 实体通用crud服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型 继承自 <see cref="BaseEntity{TKey}"/></typeparam>
    /// <typeparam name="TKey">主键类型 </typeparam>
    /// <typeparam name="TGetResponseDto">单个结果dto</typeparam>
    /// <typeparam name="TCreateRequestDto">新增实体传入的dto</typeparam>
    /// <typeparam name="TGetListRequestDto">获取列表传入的dto</typeparam>
    /// <typeparam name="TGetListResponseDto">获取列表结果的dto</typeparam>
    /// <typeparam name="TUpdateRequestDto">更新传入的dto</typeparam>
    public class EntityKeyCrudAppService<TEntity, TKey, TGetResponseDto, TGetListRequestDto, TGetListResponseDto, TUpdateRequestDto, TCreateRequestDto>
        : AlternateKeyCrudAppService<TEntity, TKey, TGetResponseDto, TGetListRequestDto, TGetListResponseDto, TUpdateRequestDto, TCreateRequestDto>
        where TEntity : BaseEntity<TKey>
        where TGetListRequestDto : PageRequestDto
    {

        protected new IRepository<TEntity, TKey> Repository { get; }


        public EntityKeyCrudAppService(IRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper) => Repository = repository;


        protected override async Task<TEntity> GetEntityByIdAsync(TKey id) => await Repository.GetAsync(id);

        protected override async Task DeleteByIdAsync(TKey id) => await Repository.DeleteAsync(id, true);


    }
}