using System.Net;
using CafeBackend.Categories.Models;
using CafeBackend.Categories.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CafeBackend.Categories.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        //readonly
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository cRepo)
        {
            _categoryRepository = cRepo;
        }

        //endpoints
        [HttpGet]
        public async Task<IActionResult> GetAllCategory( )
        {
            var categories= await _categoryRepository.ListAllCategory();

            return Ok(new{Message="Here are Categories:",categories});
        }
    

        //This can create multiple categories at the same time and also a single category.
        [HttpPost("Create")]
        public async Task<IActionResult> CreateACategory(List<CategoryCreateDto> dtos)
        {
            //Making a list so that we can add multiple categories at once
            var addedCategories = new List<Category>();

            foreach(var dto in dtos)
            {
                
                var existing = await _categoryRepository.FindCategoryByName(dto.CategoryName);
                 if(existing != null)
                {
                    continue;
                }

                 var category = new Category
                {
                    CategoryName = dto.CategoryName
                };
                
                await _categoryRepository.CreateCategory(category);
                addedCategories.Add(category);

            }

            if(addedCategories.Count == 0)
            {
                return Conflict(new{Message = "No new categories were added, all duplicates"});
            }


            return Ok(new{Message="Successfully created",Categories = addedCategories});
        }


        //Update
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateDto dto, int id)
        {
            //check if Category exist
           var existing = await _categoryRepository.FindCategoryById(id);
           if(existing == null)
            {
                return NotFound(new{Message="Category does not exists"});
            }
        
            var duplicate = await _categoryRepository.FindCategoryByName(dto.CategoryName);
            if(duplicate != null && duplicate.Id != id)
            {
                return Conflict(new{Message ="Cannot make duplicate category names,"});
            }

            existing.CategoryName = dto.CategoryName;

            await _categoryRepository.UpdateCategory(existing);
         
            return Ok(new{Message = "Successfully Updated",category = existing});
        }

        //Delete
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            //check if Category exist
           var existing = await _categoryRepository.FindCategoryById(id);
           if(existing == null)
            {
                return NotFound(new{Message="Category does not exists"});
            }
        

            await _categoryRepository.DeleteCategory(existing.Id);
         
            return Ok(new{Message = "Successfully Deleted"});
        }

        [HttpGet("Search")]
        //Search by name
        public async Task<IActionResult> Search(string searchTerm,int page=1, int pageSize=10)
        {
            
            var search = await _categoryRepository.SearchCategoriesByName(searchTerm,page,pageSize);
            return Ok(search);
        }

    }
}