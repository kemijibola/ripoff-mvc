using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace everything.Models
{
    public class RipOffLegalTeamAdvice
    {
        public int RipOffLegalTeamAdviceId { get; set; }

        [Display(Name = "Report")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Display(Name = "LegalAdvice")]
        [Required(ErrorMessage = "You must enter your advice.")]
        public string LegalAdvice { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}