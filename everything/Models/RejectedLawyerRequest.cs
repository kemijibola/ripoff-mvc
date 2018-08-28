using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class RejectedLawyerRequest
    {
        public int RejectedLawyerRequestId { get; set; }

        [Required]
        [Display(Name ="RequestId")]
        public int LawyerRequestId { get; set; }
        public virtual LawyerRequest LawyerRequest { get; set; }

        [Required]
        [Display(Name = "Reject Reason")]
        public int LawyerRejectionReasonId { get; set; }
        public virtual LawyerRejectionReason LawyerRejectionReason { get; set; }

        [Required(ErrorMessage = "You must enter date rejected.")]
        [DataType(DataType.Date)]
        public DateTime DateRejected { get; set; }
    }
}