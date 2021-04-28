using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.Entities
{
    /// <summary>
    /// 多租户
    /// </summary>
    public interface IMultiTenant
    {
        string TenantId { get; set; }
    }
}
