using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace KJCFRubberRoller.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class UpdateProfileViewModel
    {
        

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

        

    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }



    public class SetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*\d.*)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, and 1 digit")]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*\d.*)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, and 1 digit")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [RegularExpression(@"[^\s]+", ErrorMessage = "U have accidently input a whitespace. Please remove it or retype again the password.")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }


}