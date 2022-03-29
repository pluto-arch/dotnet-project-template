namespace PlutoNetCoreTemplate.Application.Models.Generics
{
    using Domain.Collections;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PageResponseDto<T> : PagedList<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PageResponseDto(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            if (source == null)
            {
                throw new NullReferenceException($"数据源不能为空");
            }

            if (pageIndex < 1)
            {
                throw new ArgumentException($"页码不能小于1");
            }

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            Items = source.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        public PageResponseDto(IEnumerable<T> source, int pageIndex, int pageSize, int total)
        {
            if (source == null)
            {
                throw new NullReferenceException($"数据源不能为空");
            }

            if (pageIndex < 1)
            {
                throw new ArgumentException($"页码不能小于1");
            }

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = total;
            Items = source.ToList();
        }



        public PageResponseDto() => Items = Array.Empty<T>();
    }
}