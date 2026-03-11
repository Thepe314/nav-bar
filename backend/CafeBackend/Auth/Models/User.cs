

using System.ComponentModel.DataAnnotations;

namespace CafeBackend.Auth.Models
{
    public class User
    {
        public int Id{get;set;}


        [Required]
        public required string FullName{get;set;}

        [EmailAddress]
        [Required]
        public required string Email{get;set;}

        [Required]
        [MinLength(8)]
        public required string Password{get;set;}

        public string Role {get;set;} = "User";

        public string? ResetToken{get;set;}

        public DateTime? ResetTokenExpiry{get;set;}

    }
}