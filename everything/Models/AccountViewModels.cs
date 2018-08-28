using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace everything.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "Last Name")]
        [StringLength(15, ErrorMessage = "Last name must be 15 characters or shorter.")]
        public string LastName { get; set; }

        [Required]
        [System.Web.Mvc.Remote("NameAlreadyExistsAsync", "Account", ErrorMessage = "Display name already exists. Please enter  a different display name.")]
        [Display(Name = "Display Name")]
        [StringLength(50, ErrorMessage = "Display name must be 50 characters or shorter.")]
        public string NameExtension { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

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

        [Required(ErrorMessage ="Please enter registered email")]
        [Display(Name = "User")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public partial class RegisterViewModell
    {

        [Phone]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "You must provide a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [System.Web.Mvc.Remote("EmailAlreadyExistsAsync", "Account", ErrorMessage = "Email already exists. Please enter a different email.")]
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

        [Display(Name = "First Name")]
        [StringLength(15, ErrorMessage = "First name must be 15 characters or shorter.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(15, ErrorMessage = "Last name must be 15 characters or shorter.")]
        public string LastName { get; set; }


        [Required]
        [System.Web.Mvc.Remote("NameAlreadyExistsAsync", "Account", ErrorMessage = "Display name already exists. Please enter  a different display name.")]
        [Display(Name = "Display Name")]
        [StringLength(50, ErrorMessage = "Display name must be 50 characters or shorter.")]
        public string NameExtension { get; set; }

        [StringLength(50, ErrorMessage = "Address must be 50 characters or shorter.")]
        public string Address { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "State")]
        public int StateId { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        [Display(Name = " I am willing to be contacted by the media, a consumer advocate or lawyer to help further my cause or to help with an investigation against the business or individual I am reporting.")]
        public bool IsInterestedInLawyer { get; set; }
        public bool DisableRP { get; set; }
    }

    public class CancelResetPassword
    {
        public string UserId { get; set; }
        public string Code { get; set; }
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
