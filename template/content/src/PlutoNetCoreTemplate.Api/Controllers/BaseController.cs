
namespace PlutoNetCoreTemplate.Api.Controllers
{
    using Domain.Aggregates.TenantAggregate;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseController<T> : ControllerBase
    {
        private readonly ILazyLoadServiceProvider _lazyLoad;

        public BaseController(ILazyLoadServiceProvider lazyLoad)
        {
            _lazyLoad = lazyLoad;
        }


        protected TService LazyGetRequiredService<TService>(ref TService reference) => _lazyLoad.LazyGetRequiredService(ref reference);

        protected TService LazyGetRequiredService<TService>() => _lazyLoad.LazyGetRequiredService<TService>();


        protected ICurrentTenant CurrentTenant => LazyGetRequiredService(ref _currentTenant);
        private ICurrentTenant _currentTenant;


        protected ILogger<T> Logger => LazyGetRequiredService(ref _logger);
        private ILogger<T> _logger;


        protected IMediator Mediator => LazyGetRequiredService(ref _mediator);
        private IMediator _mediator;

    }
}