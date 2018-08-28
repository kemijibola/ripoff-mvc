using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace everything.Areas.Rap.ViewModels
{
    public class UserandRolesViewModel
     {
        public IEnumerable<string> RoleNames { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public PagedList.IPagedList<UserInRoleModel> UserInRoleModels { get; set; }
    }
    public class UserInRoleModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public string UserId { get; set; }
    }
    public class AddUserToRoleModel
    {
        [Required]
        [Display(Name = "UserId")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Roles")]
        public string[] Roles { get; set; }
        public virtual List<ExistingRole> ExistingRole { get; set; }
    }

    public class RemoveUserFromRoleModel
    {
        [Required]
        [Display(Name = "UserId")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
        
    }
    public class ExistingRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
    public partial class RegisterAdminViewModel
    {

        [Phone]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "You must provide a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [System.Web.Mvc.Remote("EmailAlreadyExistsAsync", "ApplicationRoles", ErrorMessage = "Email already exists. Please enter a different email.")]
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
        [System.Web.Mvc.Remote("NameAlreadyExistsAsync", "ApplicationRoles", ErrorMessage = "Display name already exists. Please enter  a different display name.")]
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
}