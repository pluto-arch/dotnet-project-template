using Microsoft.AspNetCore.Identity;


namespace Pluto.netcoreTemplate.Infrastructure.Identity.Options
{
    /// <summary>
    /// Identity 框架所用的选项 参考 <see cref="IdentityOptions"/>
    /// </summary>
    public class CustomerIdentityOptions
    {
        /// <summary>
        /// 用户相关的选项  <see cref="UserOptions"/> 
        /// </summary>
        public UserOptions User { get; set; } = new UserOptions();


        /// <summary>
        /// 用户密码相关的选项 <see cref="PasswordOptions"/> 
        /// </summary>
        public PasswordOptions Password { get; set; } = new PasswordOptions();


        /// <summary>
        /// 账户锁定相关的选项 <see cref="LockoutOptions"/> 
        /// </summary>
        public LockoutOptions Lockout { get; set; } = new LockoutOptions();


        /// <summary>
        /// 用户登陆相关的选项 <see cref="SignInOptions"/> 
        /// </summary>
        public SignInOptions SignIn { get; set; } = new SignInOptions();
    }
}