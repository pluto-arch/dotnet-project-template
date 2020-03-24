using System;
using MediatR;


//  ===================
// 2020-03-24
//  ===================

namespace Pluto.netcoreTemplate.Application.Commands
{
    public class DeleteUserCommand:IRequest<bool>
    {
        public object Id { get; set; }

        public DeleteUserCommand(object id)
        {
            Id = id;
        }

    }
}