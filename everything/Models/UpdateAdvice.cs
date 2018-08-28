using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class UpdateAdvice
    {
        public int UpdateAdviceId { get; set; }

        [Display(Name = "Report Update")]
        public int ReportUpdateId { get; set; }
        public virtual ReportUpdate ReportUpdate { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Advise")]
        [Required(ErrorMessage = "You must enter advise.")]
        public string AdviseText { get; set; }


        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        //public string OriginalReport
        //{
        //    get
        //    {
        //        if(Report == null)
        //            return String.Empty;

        //        return Report.ReportText;
        //    }
 
        //}
    }
}