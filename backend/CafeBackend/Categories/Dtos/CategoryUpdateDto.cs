using System.ComponentModel.DataAnnotations;

namespace CafeBackend.Categories.Models
{
    public class CategoryUpdateDto
    {

        [Required(ErrorMessage ="Name must not be empty")]
        public string CategoryName { get; set; } = null!;

   
    }
}