using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KJCFRubberRoller.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [MaxLength(5)]
        [StringLength(5)]
        [DisplayName("Staff ID")]
        [RegularExpression(@"^[kK]\d{4}$", ErrorMessage = "Please enter the correct ID format")]
        public string StaffId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(5)]
        [StringLength(5)]
        [DisplayName("Staff ID")]
        [RegularExpression(@"^K\d{4}$", ErrorMessage = "Please enter the correct ID format")]
        public string staffID { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Name")]
        [RegularExpression(@"^[A-z\-\@\.\, ]{1,}$", ErrorMessage = "Please enter the correct name format.")]
        public string name { get; set; }

        [Required]
        [MaxLength(12)]
        [DisplayName("IC Number")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Please enter the correct IC format without \"-\". E.g. 6542125456")]
        public string IC { get; set; }

        [Required]
        [DisplayName("Position")]
        [RegularExpression(@"^[1-4]$", ErrorMessage = "Please select the correct position.")]
        public int position { get; set; }

        [Required]
        [DisplayName("Account Status")]
        public int status { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
