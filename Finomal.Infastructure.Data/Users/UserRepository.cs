using Finomal.Domain.Users;
using Finomal.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finomal.Infrastructure.Data.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.OrderBy(e => e.Id).ToListAsync();
        }

        public Task<User?> GetUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsersByIdsAsync(IEnumerable<string> userIds)
        {
            throw new NotImplementedException();
        }
    }
}
