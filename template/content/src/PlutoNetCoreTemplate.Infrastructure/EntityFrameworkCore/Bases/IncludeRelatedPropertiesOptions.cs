using PlutoNetCoreTemplate.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore
{
    public class IncludeRelatedPropertiesOptions
    {
        private readonly IDictionary<Type, object> _includeOptions = new Dictionary<Type, object>();

        public void ConfigIncludes<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> action) where TEntity : BaseEntity
        {
            _includeOptions.TryAdd(typeof(TEntity), action);
        }

        public Func<IQueryable<TEntity>, IQueryable<TEntity>> Get<TEntity>() where TEntity : BaseEntity
        {
            if (_includeOptions.TryGetValue(typeof(TEntity), out var value))
            {
                return (Func<IQueryable<TEntity>, IQueryable<TEntity>>)value;
            }

            return query => query;
        }
    }
}