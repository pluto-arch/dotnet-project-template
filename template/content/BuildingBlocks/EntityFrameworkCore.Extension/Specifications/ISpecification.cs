﻿using EntityFrameworkCore.Extension.UnitOfWork.Enums;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Extension.UnitOfWork.Specifications
{
    /// <summary>
    /// EF规约
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        /// <summary>
        /// model selector
        /// </summary>
        Expression<Func<T, TResult>> Selector { get; }
    }

    /// <summary>
    /// EF规约
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// 条件
        /// </summary>
        IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; }

        /// <summary>
        /// 排序
        /// </summary>
        IEnumerable<(Expression<Func<T, object>> KeySelector, OrderByTypeEnum OrderType)> OrderExpressions { get; }

        /// <summary>
        /// 导航属性
        /// </summary>
        IEnumerable<IncludeExpressionInfo> IncludeExpressions { get; }

        /// <summary>
        /// 导航属性
        /// </summary>
        IEnumerable<string> IncludeStrings { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<(Expression<Func<T, string>> selector, string searchTerm, int searchGroup)> SearchCriterias { get; }

        /// <summary>
        /// 
        /// </summary>
        bool CacheEnabled { get; }

        /// <summary>
        /// 
        /// </summary>
        string CacheKey { get; }

        /// <summary>
        /// by the change tracker.
        /// </summary>
        bool AsNoTracking { get; }
    }
}
