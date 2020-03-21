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
    public class RoleRepository :EfRepository<RoleEntity>, IRoleRepository
    {

        private readonly IQueryable<RoleEntity> _roleSet;
        private readonly IQueryable<UserRoleEntity> _userRoleSet;


        public RoleRepository(PlutonetcoreTemplateDbContext context):base(context)
        {
            _roleSet = context.Roles;
            _userRoleSet = context.UserRoles;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            await base.InsertAsync(role, cancellationToken);
        }
        /// <inheritdoc/>
        public async Task<RoleEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _roleSet.SingleOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }
        /// <inheritdoc/>
        public async Task<RoleEntity> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _roleSet.SingleOrDefaultAsync(x => x.RoleName == name, cancellationToken);
        }
        /// <inheritdoc/>
        public async Task<IList<RoleEntity>> GetUserRolesAsync(UserEntity user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new InvalidOperationException(nameof(user));
            }
            var userid = user.Id;
            var query = from userRole in _userRoleSet
                        join role in _roleSet on userRole.RoleId equals role.Id
                        where userRole.UserId == userid
                        select role;
            return await query.ToListAsync(cancellationToken);
        }
    }
}