using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Extension.UnitOfWork.Specifications.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class IncludableBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPreviousProperty"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="previousBuilder"></param>
        /// <param name="thenIncludeExpression"></param>
        /// <returns></returns>
        public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
            Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
            where TEntity : class
        {
            var info = new IncludeExpressionInfo(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));

            ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(info);

            var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);

            return includeBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPreviousProperty"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="previousBuilder"></param>
        /// <param name="thenIncludeExpression"></param>
        /// <returns></returns>
        public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
            Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
            where TEntity : class
        {
            var info = new IncludeExpressionInfo(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));

            ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(info);

            var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);

            return includeBuilder;
        }
    }
}
