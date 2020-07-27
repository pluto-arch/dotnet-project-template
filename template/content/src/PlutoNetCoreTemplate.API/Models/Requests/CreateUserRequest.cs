using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlutoNetCoreTemplate.API.Models.Requests
{
    /// <summary>
    /// 创建用户http request
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage ="邮箱不能为空")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage ="密码不能为空")]
        public string Password { get; set; }

    }
}