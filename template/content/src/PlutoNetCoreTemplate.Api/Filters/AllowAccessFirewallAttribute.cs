using Microsoft.AspNetCore.Mvc.Filters;

using System;

namespace PlutoNetCoreTemplate.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowAccessFirewallAttribute : Attribute, IFilterFactory, IOrderedFilter
    {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new AllowAccessFirewallAttribute();
        }
        public bool IsReusable => true;
        public int Order { get; }
    }
}