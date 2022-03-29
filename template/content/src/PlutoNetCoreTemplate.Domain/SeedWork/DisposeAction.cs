namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using System;
    using System.Threading.Tasks;

    public class DisposeAction : IDisposable
    {
        private readonly Action _action;

        public DisposeAction(Action action) => _action = action;

        void IDisposable.Dispose()
        {
            _action();
            GC.SuppressFinalize(this);
        }
    }



    public class AsyncDisposeAction : IAsyncDisposable
    {
        private readonly Action _action;

        public AsyncDisposeAction(Action action) => _action = action;

        public ValueTask DisposeAsync()
        {
            _action();
            GC.SuppressFinalize(this);
            return ValueTask.CompletedTask;
        }
    }
}
