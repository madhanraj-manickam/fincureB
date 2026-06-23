using FinCure.Data;
using FinCure.Models;
using FinCure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinCure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FinCureDBContext _context;

        public UserRepository(FinCureDBContext context)
        {
            _context = context;
        }
       
        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            // Convert to lowercase to ensure email uniqueness is not bypassed by capitalization
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<Users> AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(); // Executes the INSERT statement in SQL

            return user; // Returns the user with the newly generated ID
        }

        public async Task<Users?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}