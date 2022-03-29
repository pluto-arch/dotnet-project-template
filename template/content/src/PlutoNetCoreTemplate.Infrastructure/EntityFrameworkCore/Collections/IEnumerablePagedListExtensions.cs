using PlutoNetCoreTemplate.Domain.Collections;

using System;
using System.Collections.Generic;

namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore.Collections
{
    /// <summary>
    /// IEnumerable 扩展 <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class IEnumerablePagedListExtensions
    {
        /// <summary>
        /// 转分页： <see cref="IPagedList{T}"/> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize) => new PagedList<T>(source, pageIndex, pageSize);


        /// <summary>
        /// 转分页： <see cref="IPagedList{T}"/> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int total) => new PagedList<T>(source, pageIndex, pageSize, total);

        /// <summary>
        /// 转分页： <see cref="IPagedList{TResult}"/> 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult">结果对象</typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IPagedList<TResult> ToPagedList<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int pageIndex, int pageSize) => new PagedList<TSource, TResult>(source, converter, pageIndex, pageSize);

    }




}