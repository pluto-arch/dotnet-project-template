using MediatR;
using System.ComponentModel.DataAnnotations;

namespace PlutoNetCoreTemplate.Application.Command
{
    /// <summary>
    /// 创建账户
    /// </summary>
    //[DisableAutoSaveChangeAttribute]
    public class CreateUserCommand : BaseCommand,IRequest<bool>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "名称必填")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public CreateUserCommand()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public CreateUserCommand(string userName, string password) : this()
        {
            UserName = userName;
            Password = password;
        }

    }
}