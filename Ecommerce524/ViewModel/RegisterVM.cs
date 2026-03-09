using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

namespace Ecommerce524.ViewModel
{
    public class RegisterVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Last Name")]

        public string LName { get; set; } = string.Empty;
        [Required]

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]

        public string UserName {  get; set; } = string.Empty;
        [Required]
        //[MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {  get; set; } = string.Empty;
        [Required]

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword {  get; set; } = string.Empty;
        public string? Address { get; set; }

    }
}
