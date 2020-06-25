using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.API.Models;
using Demo.Application.GrpcServices.GrpcCallers.Interfaces;
using Demo.Infrastructure.Providers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.API.Controllers
{
    [Route("api/grpc")]
    public class GrpcController : ApiBaseController<UserController>
    {
        private readonly IOrderGrpcService _orderGrpcService;

        public GrpcController(
            IMediator mediator, 
            ILogger<UserController> logger, 
            EventIdProvider eventIdProvider, 
            IOrderGrpcService orderGrpcService) : base(mediator, logger, eventIdProvider)
        {
            _orderGrpcService = orderGrpcService;
        }

        /// <summary>
        /// get from grpc
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse> Get()
        {
            var users =await _orderGrpcService.Get();
            return ApiResponse.Success(users);
        }
    }
}
