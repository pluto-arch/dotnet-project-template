namespace PlutoNetCoreTemplate.Application.Command
{
    public class BaseCommand<TResponse> : IRequest<TResponse>
    {
        public BaseCommand()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}