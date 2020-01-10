using System.Runtime.Serialization;
using MediatR;
using Pluto.netcoreTemplate.Application.Attributes;
using Pluto.netcoreTemplate.Domain.Entities.UserAggregate;

namespace Pluto.netcoreTemplate.Application.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract, CommandTransaction]
    public class CreateUserCommand : IRequest<bool>
    {
        [DataMember]
        public string UserName { get; private set; }

        [DataMember]
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
        public CreateUserCommand(string userName,string tel):this()
        {
            UserName = userName;
            Tel = tel;

            
        }

    }
}