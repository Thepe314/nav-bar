using CafeBackend.Auth.Models;
using CafeBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBackend.Auth.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        //readonly
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //GetByEmail 
        public async Task<User?> GetbyEmail(string email)
        {
            return await _context.Users
            .FirstOrDefaultAsync(e => e.Email == email);    
             
        }

        //Create user
        public async Task CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }



    }
}