using Microsoft.EntityFrameworkCore;

using Pluto.netcoreTemplate.Domain.Entities.Account;
using Pluto.netcoreTemplate.Domain.SeedWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Pluto.netcoreTemplate.Infrastructure.Repositories.Account
{
    public class RoleRepository : IRoleRepository
    {
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        private readonly PlutonetcoreTemplateDbContext _context;
        private readonly IQueryable<RoleEntity> _roleSet;
        private readonly IQueryable<UserRoleEntity> _userRoleSet;


        public RoleRepository(PlutonetcoreTemplateDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _roleSet = _context.Roles.AsNoTracking();
            _userRoleSet = _context.UserRoles.AsNoTracking();
        }

        public async Task CreateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _context.AddAsync(role);
            ;
        }

        public async Task<RoleEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Roles.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<RoleEntity> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Roles.SingleOrDefaultAsync(x => x.RoleName == name);
        }

        public async Task<IList<RoleEntity>> GetUserRolesAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
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