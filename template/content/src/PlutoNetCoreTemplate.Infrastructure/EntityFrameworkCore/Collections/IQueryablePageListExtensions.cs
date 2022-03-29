using PlutoNetCoreTemplate.Domain.Collections;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore.Collections
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// IQueryable to IPageList Extensions
    /// </summary>
    public static class IQueryablePageListExtensions
    {
        /// <summary>
        /// 转分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException($"页码不能小于1");
            }

            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync(cancellationToken);

            var pagedList = new PagedList<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = count,
                Items = items,
            };

            return pagedList;
        }
    }
}