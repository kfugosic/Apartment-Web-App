using System.ComponentModel.DataAnnotations;

namespace ApartmanWeb.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
