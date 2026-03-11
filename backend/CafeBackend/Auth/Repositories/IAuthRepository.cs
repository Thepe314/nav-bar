using CafeBackend.Auth.Models;

namespace CafeBackend.Auth.Repositories
{
    public interface IAuthRepository
    {
        //GetByEmail 
        Task<User?> GetbyEmail(string email);


        //Create user
        Task CreateUser(User user);



        //logout



    }
}