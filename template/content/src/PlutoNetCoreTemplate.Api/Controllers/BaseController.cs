using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace PlutoNetCoreTemplate.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseController<T> : ControllerBase
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
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public BaseController(IMediator mediator, ILogger<T> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
    }
}