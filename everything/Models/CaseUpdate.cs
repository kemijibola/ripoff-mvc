using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class CaseUpdate
    {
        public int CaseUpdateId { get; set; }

        [Display(Name = "Report")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }

        [Display(Name = "Lawfirm")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int LawfirmId { get; set; }
        public virtual Lawfirm Lawfirm { get; set; }

        [Display(Name = "Case Update")]
        [Required(ErrorMessage = "You must enter case update.")]
        public string Update { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}