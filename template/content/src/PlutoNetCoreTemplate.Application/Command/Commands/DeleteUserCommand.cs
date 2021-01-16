using System;
using MediatR;


namespace PlutoNetCoreTemplate.Application.Command
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