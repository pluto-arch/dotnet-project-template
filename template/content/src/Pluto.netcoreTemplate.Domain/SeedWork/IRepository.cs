using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pluto.netcoreTemplate.Domain.SeedWork
{
    public interface IRepository<T> 
        where T :class, IAggregateRoot, new()
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 全部，用于ling 联表查询
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Entities();


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        void Insert(T entity);

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertAsync(T entity, CancellationToken cancellationToken);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
        
        /// <summary>
        /// 按条件删除
        /// </summary>
        /// <param name="whereLambda"></param>
        void Delete(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        
        /// <summary>
        /// 获取一个实体
        /// </summary>
        /// <returns></returns>
        T Get(object key);

        /// <summary>
        /// 异步获取一个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync(object key, CancellationToken cancellationToken);

        /// <summary>
        /// 按条件获取实体(不分页)
        /// </summary>
        /// <param name="whereExpress"></param>
        /// <returns></returns>
        IQueryable<T> GetList(Expression<Func<T, bool>> whereExpress);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序</param>
        /// <param name="pageIndxex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        IQueryable<T> GetPageList(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderLambda, int pageIndxex, int pageSize, out int count, bool isDesc=false);

        /// <summary>
        /// 分页异步
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="pageIndxex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<(IQueryable<T> data, int count)> GetPageListAsync(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderLambda,
            int pageIndxex, int pageSize, bool isDesc=false);

    }
}