using Microsoft.EntityFrameworkCore;
using Pluto.netcoreTemplate.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate;


namespace Pluto.netcoreTemplate.Infrastructure.Repositories.Account
{
    public class UserRepository :EfRepository<UserEntity>, IUserRepository
    {

        private readonly IQueryable<RoleEntity> _roleSet;
        private readonly IQueryable<UserEntity> _userSet;
        private readonly IQueryable<UserRoleEntity> _userRoleSet;


        public UserRepository(PlutonetcoreTemplateDbContext dbContext) : base(dbContext)
        {
            _roleSet = dbContext.Roles;
            _userSet = dbContext.Users;
            _userRoleSet = dbContext.UserRoles;
        }



        public async Task CreateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            await dbContext.Users.AddAsync(user, cancellationToken);
        }


        public async Task AddToRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var role = await _roleSet.SingleOrDefaultAsync(x => x.RoleName == roleName, cancellationToken);
            if (role == null)
            {
                throw new InvalidOperationException($"role {roleName} is not exist");
            }
            var userRole =
                await _userRoleSet.SingleOrDefaultAsync(x => x.UserId == user.Id && x.RoleId == role.Id, cancellationToken);
            if (userRole == null)
            {
                await dbContext.UserRoles.AddAsync(new UserRoleEntity
                {
                    UserId = user.Id,
                    User = user,
                    RoleId = role.Id,
                    Role = role
                });
            }
        }

        public async Task<UserEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userSet
                .Include(x=>x.Roles)
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<UserEntity> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userSet
                .Include(x => x.Roles)
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        }


        public async Task<IList<UserEntity>> GetUsersInRole(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var role = await _roleSet.SingleOrDefaultAsync(x => x.RoleName == roleName);
            if (role != null)
            {
                var query = from userrole in _userRoleSet
                            join user in _userSet on userrole.UserId equals user.Id
                            where userrole.RoleId.Equals(role.Id)
                            select user;
                return await query.ToListAsync(cancellationToken);
            }
            return new List<UserEntity>();
        }

        public async Task<bool> IsInRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("role name can not be null");
            }
            var role = await _roleSet.SingleOrDefaultAsync(x => x.RoleName == roleName, cancellationToken);
            if (role != null)
            {
                var userRole = _userRoleSet.Any(x => x.RoleId == role.Id && x.UserId == user.Id);
                return userRole;
            }
            return false;
        }


        public Task SetSecurityStampAsync(UserEntity user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ChangeSecurityStamp(stamp);
            return Task.CompletedTask;
        }

        
    }
}