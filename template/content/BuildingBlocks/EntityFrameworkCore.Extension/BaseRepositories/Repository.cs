using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace EntityFrameworkCore.Extension.UnitOfWork
{
    /// <summary>
    /// ef core 仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class Repository<TContext, TEntity> : IRepository<TEntity>
        where TEntity : class, new()
        where TContext : DbContext
    {
        private readonly TContext _dbContext;


        public virtual DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();

        /// <inheritdoc />
        public DbContext Uow => _dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(TContext dbContext)
        {
            if (_dbContext != null)
                throw new InvalidOperationException();
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public string EntityMapName
        {
            get
            {
                var entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
                return entityType.GetTableName();
            }
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        public IQueryable<TEntity> Query => DbSet.AsQueryable();

        /// <summary>
        /// IQueryable ElementType
        /// </summary>
        public Type ElementType => Query.ElementType;

        /// <summary>
        /// IQueryable Expression
        /// </summary>
        public Expression Expression => Query.Expression;

        /// <summary>
        /// IQueryProvider
        /// </summary>
        public IQueryProvider Provider => Query.Provider;


        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TEntity> GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Query.GetEnumerator();
        }

    }


}