using CafeBackend.Categories.Models;
using CafeBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBackend.Categories.Repositories
{
    public class CategoryRepository : ICategoryRepository
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
            return await _context.categories.ToListAsync();
        }

        // by Name
        public async Task <Category?> FindCategoryByName(string CategoryName)
        {
            return await _context.categories
            .FirstOrDefaultAsync(c=> c.CategoryName.ToLower() == CategoryName.ToLower());
        }

        //By id 
        public async  Task <Category?> FindCategoryById(int id)
        {
              return await _context.categories.FindAsync(id);
        }

        // Searches categories by name, prioritizing names that start with the search term,
        // then includes partial matches. Results are paginated (default 10 per page).
         public async Task<List<Category>> SearchCategoriesByName(string searchTerm, int page = 1, int pageSize =10)
        {
        
            var lowerTerm = searchTerm.Trim().ToLower();
            return await _context.categories
            .Where(c=> c.CategoryName.ToLower().Contains(lowerTerm))
            .OrderByDescending(c=> c.CategoryName.ToLower().StartsWith(lowerTerm))
            .ThenBy(c => c.CategoryName)
            .Skip((page-1)*pageSize)
            .Take(pageSize)
            .ToListAsync();
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
            //Check if it exists in database
           var category = await _context.categories.FindAsync(id);
           if(category == null)
            {
                return;
            }

            _context.categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

}