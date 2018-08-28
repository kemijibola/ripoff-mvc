using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class ReportBug
    {
        public int ReportBugId { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Bug Error")]
        [Required(ErrorMessage = "You must enter a bug error.")]
        public string Error { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}