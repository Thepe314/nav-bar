using CafeBackend.Categories.Models;
using CafeBackend.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CafeBackend.Categories.Repositories
{
    public class CategoryRepository 
    {
        //readonly

        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context; 

        }

          //List all Category
        public async Task <IEnumerable<Category>> ListAllCategory()
        {
            await _context.categories.ToListAsync();
        }

        //List by id
        public async Task <Category?> FindCategoryById(int id)
        {
            return await _context.categories.FindAsync(id);
        }

        //Create new category
       public async Task CreateCategory(Category category)
        {
             _context.categories.Add(category);
             await _context.SaveChangesAsync();
        }

        //Update
        public async Task UpdateCategory(Category category)
        {
            //find the id first
            var existing = await _context.categories.FindAsync(category.Id);
            if(existing == null)
            {
                return;
            }
                existing.CategoryName= category.CategoryName;

             await _context.SaveChangesAsync();
            
        }

        //Delete

        public async Task DeleteCategory(int id)
        {
           var category = await _context.categories.FindAsync(id);
            
            await _context.SaveChangesAsync();
        }
    }

}