namespace PlutoNetCoreTemplate.Application.Command
{
    using MediatR;

    using System;

    public class BaseCommand<TResponse> : IRequest<TResponse>
    {
        public BaseCommand()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}