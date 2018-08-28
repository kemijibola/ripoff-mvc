using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace everything.Models
{
    public class RipoffCaseUpdate
    {
        public int RipoffCaseUpdateId { get; set; }

        [Required]
        [Display(Name = "ApprovedRequestId")]
        public int ApprovedLawyerRequestId { get; set; }
        public virtual ApprovedLawyerRequest ApprovedLawyerRequest { get; set; }

        public int ReportId { get; set; }
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        [Required]
        [Display(Name = "Update Text")]
        public string UpdateText { get; set; }

        public bool Status { get; set; }
        public string AdditionalNote { get; set; }

        [Required(ErrorMessage = "You must enter date updated.")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }
    }
}