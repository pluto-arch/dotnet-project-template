namespace PlutoNetCoreTemplate.Api.Controllers
{
    using MediatR;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;


    [Route("api/account")]
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        public AccountController(IMediator mediator, ILogger<AccountController> logger) : base(mediator, logger)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">username@tenantid</param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("getToken")]
        [AllowAnonymous]
        public IActionResult GetToken([Required] string user, [Required] string role, [Required] string userId)
        {
            if (user.IndexOf('@') < 0)
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "用户名格式不正确，<username>@<tenant_id>",
                    data = ""
                });
            }
            var username = user.Split('@')[0];
            var tenantId = user.Split('@')[1];
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim("tenant_id", tenantId)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("715B59F3CDB1CF8BC3E7C8F13794CEA9"));
            var token = new JwtSecurityToken(
                issuer: "pluto",
                audience: "123",
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(30),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return Ok(new { code = 200, message = "登录成功", data = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var tenant = User.Claims.FirstOrDefault(x => x.Type == "tenant_id")?.Value;
            return Ok(new { code = 200, message = "", data = $"{user}@{tenant}" });
        }

    }
}
