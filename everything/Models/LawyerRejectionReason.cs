using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class LawyerRejectionReason
    {
        public int LawyerRejectionReasonId { get; set; }

        [Display(Name = "Reason")]
        [Required(ErrorMessage = "You must enter a reason.")]
        [StringLength(250, ErrorMessage = "The reason must be 250 characters or shorter.")]
        public string ReasonForRejection { get; set; }
    }
}