namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    public interface ILazyLoadServiceProvider
    {
        TService LazyGetRequiredService<TService>(ref TService reference);


        TService LazyGetRequiredService<TService>();
    }
}