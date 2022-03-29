﻿namespace PlutoNetCoreTemplate.Domain.Aggregates.ProductAggregate
{
    using Entities;

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 产品
    /// </summary>
    public class Product : BaseAggregateRoot<string>, IMultiTenant, ISoftDelete
    {

        public Product()
        {
            Devices = new List<Device>();
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Remark { get; set; }


        public string TenantId { get; set; }

        /// <inheritdoc />
        public bool Deleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// 设备列表
        /// </summary>
        public List<Device> Devices { get; set; }

        public void AddDevice(Device device)
        {
            this.Devices.Add(device);
        }
    }
}