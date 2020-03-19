using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Pluto.netcoreTemplate.Domain.Entities.Account;
using Pluto.netcoreTemplate.Infrastructure.Identity;
using Pluto.netcoreTemplate.Infrastructure.Identity.Options;
using Pluto.netcoreTemplate.Infrastructure.Repositories.Account;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;


namespace Pluto.netcoreTemplate.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// 添加自定义 identity
        /// 应该包括：自定义UserStore  RoleStore UserRoleStore (UserLoginStore  UserClaimStore UserLoginTokenStore)
        /// 等持久化提供程序以及UserManager、RoleManager、SignInManager 等操作提供程序
        ///
        /// 2020-1-14
        /// 暂时将UserStore RoleStore 采用仓储，对应：IUserRepository  IRoleRepository
        /// UserManager和SignManager 采用AccountServie。
        ///
        /// 后续会完全进行自定义重写Identity，用自定以的表结构 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomerIdentity(this IServiceCollection services,
            Action<CustomerIdentityOptions> setupAction)
        {
            services.AddSingleton(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
            if (setupAction != null)
            {
                services.Configure<CustomerIdentityOptions>(setupAction);
            }
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<AccountService>();


            #region Auth
            var secret = "demo security key";
            var key = Encoding.ASCII.GetBytes(secret);
            // Authentication  认证  （你是谁）
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.RequireAuthenticatedSignIn = true;
            }).AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context => OnValidateToken(context),
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                };
            });

            // Authorization  （授权 你能干什么）
            services.AddAuthorization(auth =>
            {
                // 可以添加各种策咯
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
            #endregion

            return services;
        }

        private static Task OnValidateToken(TokenValidatedContext context)
        {
            ClaimsPrincipal userPrincipal = context.Principal;
            var userIdentity= userPrincipal.FindFirstValue("id");
            var securityStemp = userPrincipal.FindFirstValue("version"); // 用户信息变更后 这里做校验 令牌失效
            //context.Fail("");
            //context.Success();
            JwtSecurityToken accessToken = context.SecurityToken as JwtSecurityToken;
            return Task.CompletedTask;
        }
    }
}