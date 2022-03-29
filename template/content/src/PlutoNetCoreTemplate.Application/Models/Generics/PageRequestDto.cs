using PlutoNetCoreTemplate.Application.AppServices.Generics;
using PlutoNetCoreTemplate.Infrastructure.Constants;

using System.Collections.Generic;

namespace PlutoNetCoreTemplate.Application.Models.Generics
{
    public class PageRequestDto
    {
        public virtual IEnumerable<SortingDescriptor> Sorter { get; set; }

        public virtual int PageNo { get; set; } = 1;

        public virtual int PageSize { get; set; } = PagingConstants.DefaultPageSize;
    }
}