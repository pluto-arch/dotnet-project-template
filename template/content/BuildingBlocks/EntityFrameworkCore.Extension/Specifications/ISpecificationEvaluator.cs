﻿using System.Linq;

namespace EntityFrameworkCore.Extension.UnitOfWork.Specifications
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecificationEvaluator<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="inputQuery"></param>
        /// <param name="specification"></param>
        /// <returns></returns>
        IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputQuery"></param>
        /// <param name="specification"></param>
        /// <returns></returns>
        IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification);
    }
}
