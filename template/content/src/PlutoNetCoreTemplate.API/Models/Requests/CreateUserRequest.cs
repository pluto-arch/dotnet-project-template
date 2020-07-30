using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlutoNetCoreTemplate.API.Models.Requests
{
	/// <summary>
	/// 创建用户http request
	/// </summary>
	public class CreateUserRequest : IValidatableObject
	{
		/// <summary>
		/// 邮箱
		/// </summary>
		[Required(ErrorMessage = "邮箱不能为空")]
		public string UserName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[Required(ErrorMessage = "密码不能为空")]
		public string Password { get; set; }

		/// <summary>Determines whether the specified object is valid.</summary>
		/// <param name="validationContext">The validation context.</param>
		/// <returns>A collection that holds failed-validation information.</returns>
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (UserName.Length < 4)
			{
				yield return new ValidationResult(
				                                  "用户名长度不够！",
				                                  new[] {nameof(UserName)});
			}

			if (Password.Length < 4)
			{
				yield return new ValidationResult(
				                                  "密码长度不够！",
				                                  new[] {nameof(Password)});
			}
		}
	}
}