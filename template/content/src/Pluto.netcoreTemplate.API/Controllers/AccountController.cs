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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace Pluto.netcoreTemplate.API.Controllers
{
    /// <summary>
    /// account controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;


        /// <summary>
        /// 
        /// </summary>
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }


        [Authorize(Roles = "admin,cus")]
        [HttpPost("Auth")]
        public IActionResult Auth()
        {
            return Ok("11212");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody]LoginRequest request)
        {
            var user = await _accountService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound(new { IsError = false, Msg = "邮箱不存在" });
            }

            var signResulrt= await _accountService.PasswordSignInAsync(user, request.Password);
            if (!signResulrt.Succeeded)
            {
                return Ok(new {IsError = true, Msg = "登陆失败"});
            }
            var userRoles = await _accountService.GetUserRolesAsync(user.Id);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("demo security key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var roleClaims = userRoles.Select(x => new Claim("Role", x.RoleName));
            tokenDescriptor.Subject.AddClaims(roleClaims);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var claimPrincipal = new ClaimsPrincipal(tokenDescriptor.Subject);
            return Ok(new { IsError = false, Msg = "", Data = new { Token = tokenString } });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp()
        {
            return Ok("");
        }


    }
}