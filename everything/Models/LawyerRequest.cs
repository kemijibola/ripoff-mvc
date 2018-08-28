using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class LawyerRequest
    {
        public int LawyerRequestId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Display(Name ="Full Name")]
        [Required(ErrorMessage ="You must enter full name.")]
        public string FullName { get; set; }

        [Display(Name = "Firm Region")]
        [Required(ErrorMessage = "You must enter firm region.")]
        public int FirmRegionId { get; set; }
        public virtual FirmRegion FirmRegion { get; set; }

        [Display(Name = "Report")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        [Required(ErrorMessage = "You must enter a phone number.")]
        [StringLength(11, ErrorMessage = "The phone number must be 11 characters or shorter.")]
        public string PhoneNumber { get; set; }
        public string AdditionalNote { get; set; }

        [Required]
        public bool AssignedToFirm { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }
    }
}