using System.ComponentModel.DataAnnotations;

namespace ApartmanWeb.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
