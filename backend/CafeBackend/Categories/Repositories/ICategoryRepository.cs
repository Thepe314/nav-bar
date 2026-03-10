using CafeBackend.Auth.Models;
using CafeBackend.Categories.Models;

namespace CafeBackend.Categories.Repositories
{
    public interface ICategoryRepository
    {
        //List all Category
        Task <IEnumerable<Category>> ListAllCategory();

        //List by id
        Task <Category?> FindCategoryById(int id);

        //Create new category
        Task CreateCategory(Category category);

        //Update
        Task UpdateCategory(Category category);

        //Delete

        Task DeleteCategory(int id);



    }
}