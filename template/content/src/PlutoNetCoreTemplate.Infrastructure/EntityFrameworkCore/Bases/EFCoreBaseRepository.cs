using Microsoft.EntityFrameworkCore;

using Pluto.Specifications;
using Pluto.Specifications.Evaluators;

using PlutoNetCoreTemplate.Domain.Entities;
using PlutoNetCoreTemplate.Domain.Repositories;

namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    using Collections;

    using Domain.Collections;
    using Domain.Exceptions;
    using Domain.UnitOfWork;

    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Options;

    using Pluto.Specifications.Exceptions;

    using PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore.Specifications.Evaluatiors;

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class EFCoreBaseRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext, IUowDbContext
        where TEntity : BaseEntity
    {
        private readonly IUnitOfWork<TDbContext> _unitOfWork;

        private TDbContext _dbContext => _unitOfWork.GetDbContext();
        private readonly ISpecificationEvaluator _specification = SpecificationEvaluator.Default;


        public EFCoreBaseRepository(IUnitOfWork<TDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IQueryable<TEntity> Query => DbSet.AsQueryable();

        public virtual DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Query.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => Query.ElementType;

        public Expression Expression => Query.Expression;

        public IQueryProvider Provider => Query.Provider;

        public IUowDbContext Uow => _dbContext;

        public virtual IAsyncQueryableProvider AsyncExecuter => new EfCoreAsyncQueryableProvider();

        public virtual async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = DbSet.Add(entity).Entity;

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return savedEntity;
        }

        public virtual async Task InsertAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(entities, cancellationToken);

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await DbSet.Where(predicate).ToListAsync(cancellationToken);

            foreach (var entity in entities)
            {
                DbSet.Remove(entity);
            }

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.RemoveRange(entities);

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _dbContext.Attach(entity);

            var updatedEntity = _dbContext.Update(entity).Entity;

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return updatedEntity;
        }

        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.UpdateRange(entities);

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await Query.CountAsync(cancellationToken);
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Query.CountAsync(predicate, cancellationToken);
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return includeDetails ? await (await IncludeRelatedAsync()).Where(predicate).SingleOrDefaultAsync(cancellationToken) : await Query.Where(predicate).SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await Query.ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> sorting, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var queryable = includeDetails ? (await IncludeRelatedAsync()) : Query;
            return await queryable.OrderBy(sorting).Where(predicate).ToListAsync(cancellationToken);
        }

        public virtual async Task<IPagedList<TEntity>> GetPageListAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> sorting, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var queryable = includeDetails ? (await IncludeRelatedAsync()) : Query;
            return await queryable.OrderBy(sorting).Where(predicate).ToPagedListAsync(pageNo, pageSize, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(predicate, includeDetails, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity));
            }

            return entity;
        }

        public virtual async Task<List<TEntity>> GetListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TResult>> GetListAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).CountAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return (await GetListAsync(specification, cancellationToken)).FirstOrDefault()!;
        }

        public virtual async Task<TResult> GetAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
        {
            return (await GetListAsync(specification, cancellationToken)).FirstOrDefault()!;
        }

        public virtual async Task<IQueryable<TEntity>> IncludeRelatedAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var includes = _dbContext.GetService<IOptions<IncludeRelatedPropertiesOptions>>().Value;

            IQueryable<TEntity> query = Query;

            if (propertySelectors is not null)
            {
                propertySelectors.ToList().ForEach(propertySelector =>
                {
                    query = query.Include(propertySelector);
                });
            }
            else
            {
                query = includes.Get<TEntity>()(query);
            }

            return await Task.FromResult(query);
        }

        public virtual async Task LoadRelatedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default) where TProperty : class
        {
            await _dbContext.Entry(entity).Collection(propertyExpression).LoadAsync(cancellationToken);
        }

        public virtual async Task LoadRelatedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default) where TProperty : class
        {
            await _dbContext.Entry(entity).Reference(propertyExpression).LoadAsync(cancellationToken);
        }


        protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return _specification.GetQuery(Query, specification);
        }

        protected IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
        {
            if (specification is null) throw new ArgumentNullException(nameof(specification));
            if (specification.Selector is null) throw new SelectorNotFoundException();

            return _specification.GetQuery(Query, specification);
        }
    }



    public class EFBaseRepository<TDbContext, TEntity, TKey> : EFCoreBaseRepository<TDbContext, TEntity>, IRepository<TEntity, TKey>
        where TDbContext : DbContext, IUowDbContext
        where TEntity : BaseEntity<TKey>
    {
        public EFBaseRepository(IUnitOfWork<TDbContext> unitOfWork) : base(unitOfWork) { }

        public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return;
            }

            await DeleteAsync(entity, autoSave, cancellationToken);
        }

        public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity));
            }

            return entity;
        }

        public async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync(new object[] { id! }, cancellationToken);
        }
    }
}