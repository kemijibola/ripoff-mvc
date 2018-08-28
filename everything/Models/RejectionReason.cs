using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class RejectionReason
    {
        public int RejectionReasonId { get; set; }

        [Display(Name = "Reason")]
        [Required(ErrorMessage = "You must enter a reason.")]
        public string Reason { get; set; }
    }
}