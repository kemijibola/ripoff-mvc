using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class Lawfirm
    {
        public int LawfirmId { get; set; }

        [Display(Name = "Firm Name")]
        [Required(ErrorMessage = "You must enter a firm name.")]
        public string FirmName { get; set; }

        [Display(Name = "Firm Holder Name")]
        [Required(ErrorMessage = "You must enter a firm holder name.")]
        public string HolderName { get; set; }

        [Display(Name = "Firm Phone Number")]
        [Phone]
        [Required(ErrorMessage = "You must enter a phone number.")]
        [StringLength(11, ErrorMessage = "The phone number must be 11 characters or shorter.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Contact Person")]
        [Required(ErrorMessage = "You must enter a contact person.")]
        public string ContactPerson { get; set; }

        [Display(Name = "Contact Phone Number")]
        [Required(ErrorMessage = "You must enter a contact phone number.")]
        [StringLength(11, ErrorMessage = "The contact phone number must be 11 characters or shorter.")]
        public string ContactNumber { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "You must enter an address.")]
        [StringLength(50, ErrorMessage = "The address must be 50 characters or shorter.")]
        public string Address { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "You must enter state id.")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "You must enter state id.")]
        public int StateId { get; set; }
        public virtual State State { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "You must enter country id.")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateRegistered { get; set; }

    }
}