using System.ComponentModel.DataAnnotations;
using MediatR;

using Pluto.netcoreTemplate.Application.Attributes;

using System.Runtime.Serialization;

namespace Pluto.netcoreTemplate.Application.Commands
{
    /// <summary>
    /// 创建账户
    /// </summary>
    public class CreateUserCommand : IRequest<bool>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; private set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Tel { get; private set; }


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
        /// <param name="tel"></param>
        public CreateUserCommand(string userName, string tel) : this()
        {
            UserName = userName;
            Tel = tel;
        }

    }
}