using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
