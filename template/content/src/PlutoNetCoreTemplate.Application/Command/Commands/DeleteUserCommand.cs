using System;
using MediatR;

using PlutoNetCoreTemplate.Application.Attributes;

namespace PlutoNetCoreTemplate.Application.Command
{
    [DisableAutoSaveChange]
    public class DeleteUserCommand:BaseCommand,IRequest<bool>
    {
        public object Id { get; set; }

        public DeleteUserCommand(object id)
        {
            Id = id;
        }

    }
}