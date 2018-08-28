using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class ReportRejection
    {
        public int ReportRejectionId { get; set; }

        [Display(Name = "Report")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }

        [Display(Name = "Rejection Reason")]
        [Required(ErrorMessage = "You must enter rejection id.")]
        public int RejectionReasonId { get; set; }
        public virtual RejectionReason RejectionReason { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}