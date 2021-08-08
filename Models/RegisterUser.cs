using System.ComponentModel.DataAnnotations;

namespace TeamAPI.Models
{
    public class RegisterUser
    {
        [Required,MinLength(2)]
        public string UserName { get; set; }
        [EmailAddress]
        public  string Email { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        
        
    }
}