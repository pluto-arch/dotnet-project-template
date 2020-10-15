using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlutoNetCoreTemplate.Infrastructure.Providers;


namespace PlutoNetCoreTemplate.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseController<T>:ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        internal readonly IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        internal readonly ILogger _logger;
        /// <summary>
        /// 
        /// </summary>
        internal readonly EventIdProvider _eventIdProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="eventIdProvider"></param>
        public BaseController(IMediator mediator, ILogger<T> logger, EventIdProvider eventIdProvider)
        {
            _mediator = mediator;
            _logger = logger;
            _eventIdProvider = eventIdProvider;
        }
    }
}