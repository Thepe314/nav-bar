

using System.ComponentModel.DataAnnotations;

namespace CafeBackend.Auth.Dtos
{
    public class LoginDto
    {
       

        [EmailAddress(ErrorMessage ="Email is invalid")]
        [Required(ErrorMessage ="Email is required")]
        public required string Email{get;set;}

       
        [Required(ErrorMessage = "Password is Required")]
        [MinLength(8,ErrorMessage = "Password must be at least 8 characters")]
        public required string Password{get;set;}


    }
}