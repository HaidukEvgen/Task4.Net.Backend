using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task4.Backend.Data;
using Task4.Backend.Infrastructure.Enums;
using Task4.Backend.Models.Data;

namespace Task4.Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly DbSet<User> _dbSet;
        private readonly UserManager<User> _userManager;

        public UserRepository(UserDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _dbSet = _context.Set<User>();
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<bool> IsPasswordCorrectAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task SetStatusesAsync(List<string> userIds, Status status)
        {
            var usersToUpdate = await _dbSet.Where(u => userIds.Contains(u.Id)).ToListAsync();

            foreach (var user in usersToUpdate)
            {
                user.Status = status;
            }

            await SaveChangesAsync();
        }


        public async Task DeleteUsersAsync(List<string> userIds)
        {
            var usersToDelete = await _dbSet.Where(u => userIds.Contains(u.Id)).ToListAsync();

            _dbSet.RemoveRange(usersToDelete);

            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
