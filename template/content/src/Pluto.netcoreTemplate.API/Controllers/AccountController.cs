using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Pluto.netcoreTemplate.API.Models.Requests;
using Pluto.netcoreTemplate.Infrastructure.Identity;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        private readonly AccountService _accountService;

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
            EventIdProvider eventIdProvider, AccountService accountService) : base(mediator, logger, eventIdProvider)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<ApiResponse> TokenAsync([FromBody]LoginRequest request)
        {
            var user = await _accountService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return ApiResponse.DefaultFail("用户不存在");
            }
            var signResulrt= await _accountService.PasswordSignInAsync(user, request.Password);
            if (!signResulrt.Succeeded)
            {
                return ApiResponse.DefaultFail("登陆失败");
            }
            var userRoles = await _accountService.GetUserRolesAsync(user.Id);
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("demo security key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.UserName),
                    new Claim("version", user.SecurityStamp)
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var roleClaims = userRoles.Select(x => new Claim(ClaimTypes.Role, x.RoleName));
            tokenDescriptor.Subject.AddClaims(roleClaims);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return ApiResponse.Success(new {Token= tokenString,Expires= tokenDescriptor.Expires });
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
                var ress= ApiResponse.DefaultSuccess("创建成功");
                return ress;
            }
            return ApiResponse.DefaultFail("创建失败");
        }

    }
}