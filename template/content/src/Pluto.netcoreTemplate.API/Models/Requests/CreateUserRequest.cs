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
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        ///  先这里验证  在进行ModelValidateFilter 所以这个过滤器 没什么用了 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (string.IsNullOrEmpty(Email)|| Email.Length<10)
                yield return new ValidationResult("Email is not allowed null");
        }
    }
}