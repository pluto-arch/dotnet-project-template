using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pluto.netcoreTemplate.Domain;
using Pluto.netcoreTemplate.Domain.SeedWork;


namespace Pluto.netcoreTemplate.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T>
        where T : class, IAggregateRoot, new()
    {


        internal readonly PlutonetcoreTemplateDbContext dbContext;

        private DbSet<T> dbSet;

        public IUnitOfWork UnitOfWork => dbContext;


        public EfRepository(PlutonetcoreTemplateDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }


        /// <inheritdoc />
        public IQueryable<T> Entities()
        {
            return dbSet;
        }


        /// <inheritdoc />
        public virtual void Insert(T entity)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            dbSet.Add(entity);

        }

        /// <inheritdoc />
        public virtual async Task InsertAsync(T entity, CancellationToken cancellationToken)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbSet.AddAsync(entity);
        }


        /// <inheritdoc />
        public virtual void Delete(T entity)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            dbSet.Remove(entity);

        }

        /// <inheritdoc />
        public virtual void Delete(Expression<Func<T, bool>> whereLambda)
        {
            if (null == whereLambda)
            {
                throw new ArgumentNullException("条件不能为空");
            }
            dbSet.RemoveRange(dbSet.Where(whereLambda));
        }

        /// <inheritdoc />
        public virtual void Update(T entity)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            dbContext.Entry(entity).State = EntityState.Modified;
        }


        /// <inheritdoc />
        public virtual T Get(object key)
        {
            return dbSet.Find(key);
        }



        /// <inheritdoc />
        public virtual async Task<T> GetAsync(object key, CancellationToken cancellationToken)
        {
            return await dbSet.FindAsync(key);
        }


        /// <inheritdoc />
        public virtual IQueryable<T> GetList(Expression<Func<T, bool>> whereExpress)
        {
            return dbSet.Where(whereExpress);
        }


        /// <inheritdoc />
        public virtual IQueryable<T> GetPageList(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderLambda, int pageIndxex, int pageSize, out int count, bool isDesc = false)
        {
            count = dbSet.Count();
            if (count == 0)
                return default;
            if (whereLambda == null)
                whereLambda = x => true;
            if (orderLambda == null)
                orderLambda = x => new { };
            if (isDesc)
            {
                return dbSet.Where(whereLambda).OrderByDescending(orderLambda).Skip((pageIndxex - 1) * pageSize).Take(pageSize);
            }
            return dbSet.Where(whereLambda).OrderBy(orderLambda).Skip((pageIndxex - 1) * pageSize).Take(pageSize);
        }

        /// <inheritdoc />
        public virtual async Task<(IQueryable<T> data, int count)> GetPageListAsync(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderLambda,
            int pageIndxex, int pageSize, bool isDesc = false)
        {
            var count = await dbSet.CountAsync();
            if (count == 0)
                return default;
            if (whereLambda == null)
                whereLambda = x => true;
            if (orderLambda == null)
                orderLambda = x => new { };
            if (isDesc)
            {
                return (dbSet.Where(whereLambda).OrderByDescending(orderLambda).Skip((pageIndxex - 1) * pageSize).Take(pageSize), count);
            }
            return (dbSet.Where(whereLambda).OrderBy(orderLambda).Skip((pageIndxex - 1) * pageSize).Take(pageSize), count);
        }
        
    }
}