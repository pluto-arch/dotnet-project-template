namespace PlutoNetCoreTemplate.Infrastructure.Providers
{
    using System;

    public interface ILazyLoadServiceProvider
    {
        TService LazyGetRequiredService<TService>(ref TService reference);


        TService LazyGetRequiredService<TService>();
    }
}