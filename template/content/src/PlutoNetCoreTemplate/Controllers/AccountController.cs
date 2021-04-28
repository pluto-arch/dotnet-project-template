namespace PlutoNetCoreTemplate.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;


    [Route("api/account")]
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        public AccountController(IMediator mediator, ILogger<AccountController> logger) : base(mediator, logger)
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetToken(string user,string role,string userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role)
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
    }
}
