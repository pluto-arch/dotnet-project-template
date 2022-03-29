namespace PlutoNetCoreTemplate.Application.Models.ProductModels
{
    /// <summary>
    /// 产品输出模型
    /// </summary>
    public class ProductGetResponseDto
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
    }
}