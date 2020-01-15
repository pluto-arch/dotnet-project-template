using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using Pluto.netcoreTemplate.Domain.Entities.Account;
using Pluto.netcoreTemplate.Infrastructure.Identity.Options;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Pluto.netcoreTemplate.Infrastructure.Identity
{
    /// <summary>
    /// Account 相关查询
    /// </summary>
    public class AccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        private readonly IPasswordHasher<UserEntity> _passwordHasher;

        private readonly CustomerIdentityOptions identityOptions;

        /// <summary>
        /// 用于task的取消
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="passwordHasher"></param>
        public AccountService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPasswordHasher<UserEntity> passwordHasher,
            IOptions<CustomerIdentityOptions> option)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            identityOptions = option?.Value;
        }


        protected async Task<IdentityResult> ValidateUserAsync(UserEntity user)
        {
            var errors = new List<IdentityError>();
            if (identityOptions.User.RequireUniqueEmail)
            {
                if (await _userRepository.GetByEmailAsync(user.Email, CancellationToken) != null)
                {
                    errors.Add(new IdentityError { Code = "Invalid Email", Description = "邮箱已被占用" });
                }
            }

            if (!string.IsNullOrEmpty(identityOptions.User.AllowedUserNameCharacters))
            {
                // todo user name characters validate
            }

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
        protected async Task<IdentityResult> ValidatePassWordAsync(string password)
        {
            var errors = new List<IdentityError>();
            // todo validate password with identityOptions.Password
            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateUserAsync(string userName, string email, string password)
        {
            var user = new UserEntity
            {
                UserName = userName,
                Email = email,
            };
            var userValid = await ValidateUserAsync(user);
            if (!userValid.Succeeded)
            {
                return userValid;
            }
            var passwdValidate = await ValidatePassWordAsync(password);
            if (!passwdValidate.Succeeded)
            {
                return passwdValidate;
            }
            var passwordhash = _passwordHasher.HashPassword(user, password);
            user.SetPasswordHash(passwordhash);
            user.SetSecurityStamp(NewSecurityStamp());
            await _userRepository.CreateAsync(user, CancellationToken);
            await _userRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken);
            return IdentityResult.Success;
        }

        public async Task<SignInResult> PasswordSignInAsync(UserEntity user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.LockoutEnabled)
            {
                return SignInResult.LockedOut;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Success)
            {
                // todo 如果有锁定  则登陆成功后 失败次数置为0
                // todo 如果有双重认证  则使用双重认证规则处理
                return SignInResult.Success;
            }
            // todo 如果失败 可以增加失败次数  然后根据此值进行锁定用户
            return SignInResult.Failed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserEntity> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id, CancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email, CancellationToken);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateRoleAsync(string userName, string email, string password)
        {
            await _roleRepository.CreateAsync(new RoleEntity
            {
                RoleName = userName,
            }, CancellationToken);
            await _roleRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken);
            return IdentityResult.Success;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RoleEntity> GetRoleByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id, CancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<RoleEntity> GetRoleByNameAsync(string roleName)
        {
            return await _roleRepository.GetByNameAsync(roleName, CancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IList<RoleEntity>> GetUserRolesAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"{userId} does not exist");
            }

            return await _roleRepository.GetUserRolesAsync(user, CancellationToken);
        }



        private async Task UpdateSecurityStampInternal(UserEntity user)
        {
            user.SetSecurityStamp(NewSecurityStamp());
            await _userRepository.UnitOfWork.SaveEntitiesAsync(CancellationToken);
        }
        private static string NewSecurityStamp()
        {
            byte[] bytes = new byte[20];
            _rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}