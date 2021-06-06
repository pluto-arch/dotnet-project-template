namespace PlutoNetCoreTemplate.Domain.Aggregates.SystemAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Entities;

    /// <summary>
    /// 权限分组定义
    /// </summary>
    public class PermissionGroupDefinition:BaseEntity<int>
    {

        public PermissionGroupDefinition()
        {
            
        }

        /// <inheritdoc />
        public PermissionGroupDefinition(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }


        /// <summary>
        /// 接口地址
        /// </summary>
        public string Url { get; set; }


        /// <summary>
        /// 上级分组
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public List<PermissionDefinition> Permissions { get; set; }

    }
}