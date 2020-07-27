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
            _logger.LogInformation(_eventIdProvider.EventId,"开始调用Grpc服务");
            var users =await _orderGrpcService.Get();
            _logger.LogInformation(_eventIdProvider.EventId, "调用Grpc服务完成，返回值：{@users}",users);
            return ApiResponse.Success(users);
        }
    }
}
