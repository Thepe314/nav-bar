
using CafeBackend.Auth.Models;
using CafeBackend.Categories.Models;
using CafeBackend.Models;
using Microsoft.EntityFrameworkCore;


namespace CafeBackend.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {
            
        }

        //Tables   
        public DbSet<User> Users {get;set;}

        public DbSet<Category> categories{get;set;}

    }
}
