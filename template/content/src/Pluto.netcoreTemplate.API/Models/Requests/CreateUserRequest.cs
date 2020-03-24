using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pluto.netcoreTemplate.API.Models.Requests
{
    /// <summary>
    /// 创建用户http request
    /// </summary>
    public class CreateUserRequest : IValidatableObject
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


        /// <summary>
        ///  可以使用特性，也可以使用这里，这里做关联验证
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (string.IsNullOrEmpty(UserName) || UserName.Length<10)
                yield return new ValidationResult("Email is not allowed null");
        }
    }
}