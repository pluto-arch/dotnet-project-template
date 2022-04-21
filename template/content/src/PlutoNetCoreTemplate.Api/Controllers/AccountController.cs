namespace PlutoNetCoreTemplate.Api.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    using PlutoNetCoreTemplate.Api.Constants;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using Domain.SeedWork;


    [Route("api/account")]
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        public AccountController(ILazyLoadServiceProvider lazyLoad) : base(lazyLoad)
        {
        }



        private static readonly List<dynamic> Users = new()
        {
            new
            {
                Id = 1,
                Mobile = "18530064433",
                UserName = "admin3",
                Password = "admin",
                Role = "admin",
                TenantId = "T20210602000003"
            },
            new
            {
                Id = 2,
                Mobile = "18530064432",
                UserName = "admin2",
                Password = "admin",
                Role = "admin",
                TenantId = "T20210602000002"
            },
            new
            {
                Id = 3,
                Mobile = "18530064431",
                UserName = "sa",
                Password = "admin",
                Role = "SystemAdmin",
                TenantId = "T20210602000001"
            }
        };


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("getToken")]
        [AllowAnonymous]
        public ServiceResponse<string> GetToken(
            [Required, FromForm(Name = "userName")] string user,
            [Required, FromForm(Name = "password")] string pwd)
        {
            var u = Users.FirstOrDefault(x => x.UserName == user && x.Password == pwd);
            if (u == null)
            {
                return ServiceResponse<string>.Error("用户不存在");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, u.UserName),
                new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
                new Claim(ClaimTypes.Role, u.Role),
                new Claim(TenantClaimTypes.TenantId, u.TenantId)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("715B59F3CDB1CF8BC3E7C8F13794CEA9"));
            var token = new JwtSecurityToken(
                issuer: "pluto",
                audience: "123",
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(120),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return ServiceResponse<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
        }

    }
}
