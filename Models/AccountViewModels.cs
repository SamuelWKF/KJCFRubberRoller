using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KJCFRubberRoller.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z\.]+)$")]
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
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z\.]+)$")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [MaxLength(5)]
        [StringLength(5)]
        [DisplayName("Staff ID")]
        [RegularExpression(@"^([kK])\d+$", ErrorMessage = "Staff ID must start with K & follow by maximum 4 digits")]
        public string StaffId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        
    }

    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z\.]+)$")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
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
        [RegularExpression(@"^([kK])\d+$", ErrorMessage = "Staff ID must start with K & follow by maximum 4 digits")]
        public string staffID { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Name")]
        [RegularExpression(@"^[A-z\-\@\.\, ]{1,}$", ErrorMessage = "Please enter the correct name format.")]
        public string name { get; set; }

        [Required]
        [MaxLength(12,ErrorMessage = "Please enter the correct IC format without \"-\". E.g. 651212015591")]
        [DisplayName("IC Number")]
        [RegularExpression(@"^\d{12,}$", ErrorMessage = "Please enter the correct IC format without \"-\". E.g. 651212015591")]
        public string IC { get; set; }

        [Required]
        [DisplayName("Position")]
        [RegularExpression(@"^[1-4]$", ErrorMessage = "Please select the correct position.")]
        public int position { get; set; }

        [Required]
        [DisplayName("Account Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Please select the correct status.")]
        public int status { get; set; }
        
    }

    public class UpdateViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z\.]+)$")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(5)]
        [StringLength(5)]
        [DisplayName("Staff ID")]
        [RegularExpression(@"^([kK])\d+$", ErrorMessage = "Staff ID must start with K & follow by maximum 4 digits")]
        public string staffID { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Name")]
        [RegularExpression(@"^[A-z\-\@\.\, ]{1,}$", ErrorMessage = "Please enter the correct name format.")]
        public string name { get; set; }

        [Required]
        [MaxLength(12, ErrorMessage = "Please enter the correct IC format without \"-\". E.g. 651212015591")]
        [DisplayName("IC Number")]
        [RegularExpression(@"^\d{12,}$", ErrorMessage = "Please enter the correct IC format without \"-\". E.g. 651212015591")] 
        public string IC { get; set; }

        [Required]
        [DisplayName("Position")]
        [RegularExpression(@"^[1-4]$", ErrorMessage = "Please select the correct position.")]
        public int position { get; set; }

        [Required]
        [DisplayName("Account Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Please select the correct status.")]
        public int status { get; set; }

        [DisplayName("Reset Passsword")]
        public bool isReset { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z\.]+)$")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*\d.*)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, and 1 digit")]
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
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z\.]+)$")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

   

}
