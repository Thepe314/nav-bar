using System.ComponentModel.DataAnnotations;

namespace CafeBackend.Categories.Models
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage ="Name must not be empty")]
        public string CategoryName { get; set; } = null!;

   
    }
}