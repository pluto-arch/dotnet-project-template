using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pluto.netcoreTemplate.API.Models.Requests;

using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Pluto.netcoreTemplate.API.Models;
using Pluto.netcoreTemplate.API.Models.Responses;
using Pluto.netcoreTemplate.Application.Commands;
using Pluto.netcoreTemplate.Infrastructure.Providers;

namespace Pluto.netcoreTemplate.API.Controllers
{
    /// <summary>
    /// account controller
    /// </summary>
    [Route("api/account")]
    [ApiController]
    public class AccountController : ApiBaseController<AccountController>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="eventIdProvider"></param>
        /// <param name="accountService"></param>
        public AccountController(
            IMediator mediator,
            ILogger<AccountController> logger,
            EventIdProvider eventIdProvider) : base(mediator, logger, eventIdProvider)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<ApiResponse> TokenAsync([FromBody]LoginRequest request)
        {
            return ApiResponse.Success(new { Token = "", Expires = 360000 });
        }


        /// <summary>
        /// 创建账户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> PostAsync([FromBody]LoginRequest request)
        {
            var res = await _mediator.Send(new CreateUserCommand(request.Email, request.Password));
            if (res)
            {
                var ress = ApiResponse.DefaultSuccess("创建成功");
                return ress;
            }
            return ApiResponse.DefaultFail("创建失败");
        }

    }
}