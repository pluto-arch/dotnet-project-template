namespace Pluto.netcoreTemplate.API.Models.Requests
{
    /// <summary>
    /// 创建用户http request
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Tel { get; set; }
    }
}