namespace PlutoNetCoreTemplate.Application.Models.ProductModels
{
    using System.Collections.Generic;

    /// <summary>
    /// 产品
    /// </summary>
    public class ProductModels
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Remark { get; set; }


        public List<DeviceModel> Devices { get; set; }
    }
}