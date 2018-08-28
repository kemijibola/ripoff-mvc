using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class ApprovedLawyerRequest
    {
        public int ApprovedLawyerRequestId { get; set; }

        [Required]
        public int LawyerRequestId { get; set; }
        public virtual LawyerRequest LawyerRequest { get; set; }

        [Required]
        public int LawFirmId { get; set; }
        public virtual Lawfirm Lawfirm { get; set; }

        [Required(ErrorMessage = "You must enter date approved.")]
        [DataType(DataType.Date)]
        public DateTime DateApproved { get; set; }
    }
}