using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class ReportUpdate
    {
        public int ReportUpdateId { get; set; }

        [Display(Name = "Report")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Update")]
        [Required(ErrorMessage = "You must enter an update.")]
        public string Update { get; set; }

        [Required]
        public bool AdviseStatus { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public string ReportTitle
        {
            get
            {
                if (Report == null)
                    return String.Empty;

                return Report.Title;
            }
        }

        public virtual ICollection<UpdateAdvice> UpdateAdvices { get; set; }

    }
}