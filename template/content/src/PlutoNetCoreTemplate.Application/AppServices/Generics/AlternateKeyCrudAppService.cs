namespace PlutoNetCoreTemplate.Application.AppServices.Generics
{
    using AutoMapper;

    using Domain.Entities;

    using Models.Generics;

    using PlutoNetCoreTemplate.Domain.Repositories;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    /// <summary>
    /// 实体通用crud服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型 继承自 <see cref="BaseEntity"/></typeparam>
    /// <typeparam name="TKey">主键类型 复合主键可以是object</typeparam>
    /// <typeparam name="TGetResponseDto">单个结果dto</typeparam>
    /// <typeparam name="TCreateRequestDto">新增实体传入的dto</typeparam>
    /// <typeparam name="TGetListRequestDto">获取列表传入的dto</typeparam>
    /// <typeparam name="TGetListResponseDto">获取列表结果的dto</typeparam>
    /// <typeparam name="TUpdateRequestDto">更新传入的dto</typeparam>
    public abstract class AlternateKeyCrudAppService<TEntity, TKey, TGetResponseDto, TGetListRequestDto, TGetListResponseDto, TUpdateRequestDto, TCreateRequestDto> :
        IGetApService<TKey, TGetResponseDto>,
        IGetListAppService<TGetListRequestDto, TGetListResponseDto>,
        ICreateAppService<TCreateRequestDto, TGetResponseDto>,
        IDeleteAppService<TKey>,
        IUpdateAppService<TKey, TUpdateRequestDto, TGetResponseDto>
        where TEntity : BaseEntity
    where TGetListRequestDto : PageRequestDto
    {

        protected IRepository<TEntity> Repository { get; }

        protected IMapper Mapper { get; }


        public AlternateKeyCrudAppService(IRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }


        public virtual async Task<TGetResponseDto> CreateAsync(TCreateRequestDto requestModel)
        {
            TEntity entity = Mapper.Map<TEntity>(requestModel);
            await Repository.InsertAsync(entity, true);
            return Mapper.Map<TGetResponseDto>(entity);
        }

        public virtual async Task<TGetResponseDto> GetAsync(TKey id) => Mapper.Map<TGetResponseDto>(await GetEntityByIdAsync(id));

        public virtual async Task<PageResponseDto<TGetListResponseDto>> GetListAsync(TGetListRequestDto model)
        {
            IQueryable<TEntity> query = CreateFilteredQuery(model);
            int totalCount = await Repository.AsyncExecuter.CountAsync(query);
            query = ApplySorting(query, model);
            query = ApplyPaging(query, model);
            var entities = await Repository.AsyncExecuter.ToListAsync(query);
            return new PageResponseDto<TGetListResponseDto>(Mapper.Map<List<TGetListResponseDto>>(entities), model.PageNo, model.PageSize, totalCount);
        }

        public virtual async Task DeleteAsync(TKey id) => await DeleteByIdAsync(id);

        public async Task<TGetResponseDto> UpdateAsync(TKey id, TUpdateRequestDto requestModel)
        {
            TEntity entity = await GetEntityByIdAsync(id);
            Mapper.Map(requestModel, entity);
            await Repository.UpdateAsync(entity, true);
            return Mapper.Map<TGetResponseDto>(entity);
        }





        protected abstract Task<TEntity> GetEntityByIdAsync(TKey id);

        protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetListRequestDto requestModel) => Repository.Query;

        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetListRequestDto requestModel)
        {
            if (requestModel is PageRequestDto pagedRequestModel && pagedRequestModel.Sorter is not null && pagedRequestModel.Sorter.Any())
            {
                var properties = query.GetType().GetGenericArguments().First().GetProperties();

                IOrderedQueryable<TEntity> orderedQueryable = null;

                foreach (SortingDescriptor sortingDescriptor in pagedRequestModel.Sorter)
                {
                    string propertyName = properties.SingleOrDefault(p => string.Equals(p.Name, sortingDescriptor.PropertyName, StringComparison.OrdinalIgnoreCase))?.Name;

                    if (propertyName is null)
                    {
                        throw new KeyNotFoundException(sortingDescriptor.PropertyName);
                    }

                    if (sortingDescriptor.SortDirection == SortingOrder.Ascending)
                    {
                        orderedQueryable = orderedQueryable is null ? query.OrderBy(propertyName) : orderedQueryable.ThenBy(propertyName);
                    }
                    else if (sortingDescriptor.SortDirection == SortingOrder.Descending)
                    {
                        orderedQueryable = orderedQueryable is null ? query.OrderByDescending(propertyName) : orderedQueryable.ThenByDescending(propertyName);
                    }
                }

                return orderedQueryable ?? query;
            }
            return query;
        }

        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetListRequestDto requestModel)
        {
            if (requestModel is PageRequestDto model)
            {
                return query.Skip((model.PageNo - 1) * model.PageSize).Take(model.PageSize);
            }

            return query;
        }

        protected abstract Task DeleteByIdAsync(TKey id);


    }
}