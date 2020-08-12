using System;
using MediatR;


//  ===================
// 2020-03-24
//  ===================

namespace PlutoNetCoreTemplate.Application.CommandBus.Commands
{
    public class DeleteUserCommand:BaseCommand,IRequest<bool>
    {
        public object Id { get; set; }

        public DeleteUserCommand(object id)
        {
            Id = id;
        }

    }
}