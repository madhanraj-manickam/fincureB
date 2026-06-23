using FinCure.Models;

namespace FinCure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        // Used to check for duplicate emails during Registration and verifying Login credentials
        Task<Users?> GetUserByEmailAsync(string email);

        // Used to save the new user to the database
        Task<Users> AddUserAsync(Users user);

        // Standard best-practice method to retrieve a user by their primary key
        Task<Users?> GetUserByIdAsync(int id);
    }
}