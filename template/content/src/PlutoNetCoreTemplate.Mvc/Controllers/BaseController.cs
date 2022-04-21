namespace PlutoNetCoreTemplate.Mvc.Controllers
{
    using Domain.Aggregates.TenantAggregate;
    using Domain.SeedWork;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class BaseController<T> : Controller
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