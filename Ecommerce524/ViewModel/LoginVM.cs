using System.ComponentModel.DataAnnotations;

namespace Ecommerce524.ViewModel
{
    public class LoginVM
    {
        public int Id { get; set; }
        [Required]
        //[EmailAddress]
        [Display(Name = "Email or Username")]
        public string EmailOrUserName { get; set; } = string.Empty;
        [Required]
        //[MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
