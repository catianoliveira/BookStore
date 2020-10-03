using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class CreateRoleViewModel
    {
        public string Id { get; set; }


        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
