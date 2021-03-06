using System;
using MediatR;

using PlutoNetCoreTemplate.Infrastructure.Commons;

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