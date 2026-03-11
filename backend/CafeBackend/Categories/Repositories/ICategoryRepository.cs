
using CafeBackend.Categories.Models;

namespace CafeBackend.Categories.Repositories
{
    public interface ICategoryRepository
    {
        //List all Category
        Task <IEnumerable<Category>> ListAllCategory();

        // Category by name
        Task <Category?> FindCategoryByName(string CategoryName);

        //Categories
        Task<List<Category>> SearchCategoriesByName(string searchTerm, int page = 1, int pageSize =10);

        //by id
        Task <Category?> FindCategoryById(int id);

        //Create new category
        Task CreateCategory(Category category);

        //Update
        Task UpdateCategory(Category category);

        //Delete

        Task DeleteCategory(int id);



    }
}