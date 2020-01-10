using System;
using System.Linq;
using Pluto.netcoreTemplate.Domain.Entities.UserAggregate;
using Pluto.netcoreTemplate.Domain.SeedWork;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pluto.netcoreTemplate.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        private readonly PlutonetcoreTemplateDbContext _context;

        public UserRepository(PlutonetcoreTemplateDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public User Add(User user)
        {
            return _context.Users.Add(user).Entity;
        }

        public async Task<User> GetAsync(int userId)
        {
            var user = await _context
                .Users
                .FirstOrDefaultAsync(o => o.Id == userId);
            if (user == null)
            {
                user = _context
                    .Users
                    .Local
                    .FirstOrDefault(o => o.Id == userId);
            }
            return user;
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<(int total, IQueryable<User> list)> GetListAsync(int page, int rows)
        {
            var count = await _context.Users.AsNoTracking().CountAsync();
            var list = _context.Users.Take(rows).Skip((page - 1) * rows);
            return (count, list);
        }
    }
}