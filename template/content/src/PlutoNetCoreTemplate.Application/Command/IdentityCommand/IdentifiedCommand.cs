using MediatR;

using System;

namespace PlutoNetCoreTemplate.Application.Command
{
    /// <summary>
    /// 具有辨识的command。执行前会进行查重
    /// </summary>
    /// <typeparam name="T">需要执行的command</typeparam>
    /// <typeparam name="R">T对应的返回类型</typeparam>
    public class IdentifiedCommand<T, R> : IRequest<R>
        where T : IRequest<R>
    {
        public T Command { get; }
        public Guid Id { get; }
        public IdentifiedCommand(T command)
        {
            Command = command;
            Id = Guid.NewGuid();
        }
    }
}