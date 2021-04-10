using System;
using MediatR;

using PlutoNetCoreTemplate.Infrastructure.Commons;

namespace PlutoNetCoreTemplate.Application.Command
{
    [DisableAutoSaveChange]
    public class DeleteUserCommand:BaseCommand,IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }

    }
}