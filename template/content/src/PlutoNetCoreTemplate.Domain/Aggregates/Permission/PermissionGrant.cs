using System;

namespace PlutoNetCoreTemplate.Domain.Aggregates.Permission
{
    using Entities;

    /// <summary>
    /// 对应的权限授予信息
    /// </summary>
    public class PermissionGrant:BaseAggregateRoot<int>,IMultiTenant
    {

        /*
         * TenantId=10
         * Name=product.create
         * ProviderName=role
         * ProviderKey=admin
         *
         * 租户10 中产品创建 使用 role进行权限授予，拥有admin的角色才有权限
         */

        /// <inheritdoc />
        public string TenantId { get; set; }


        /// <summary>
        /// 权限名称
        /// 例如：  产品表的增加权限：product.create
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 被授权主体名称
        /// 例如：角色、部门
        /// </summary>
        public string ProviderName { get; set; }


        /// <summary>
        /// 主体的标识
        /// </summary>
        public string ProviderKey { get; set; }

        public DateTimeOffset CreateTime { get; set; }

    }
}