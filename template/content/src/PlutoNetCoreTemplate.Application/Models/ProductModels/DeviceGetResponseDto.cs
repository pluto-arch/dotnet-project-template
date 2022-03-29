namespace PlutoNetCoreTemplate.Application.Models.ProductModels
{
    public class DeviceGetResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string SerialNo { get; set; }

        /// <summary>
        /// 经纬度
        /// </summary>
        public string Coordinate { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool Online { get; set; }

    }
}