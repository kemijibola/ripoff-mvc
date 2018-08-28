using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class Thread
    {
        public int ThreadId { get; set; }

        [Display(Name = "Report")]
        [Required(ErrorMessage = "You must enter report.")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }

        [Display(Name = "User Id")]
        [Required(ErrorMessage = "You must enter user.")]
        public string UserId { get; set; }

        [Display(Name = "ThreadText")]
        [Required(ErrorMessage = "You must enter a comment.")]
        [AllowHtml]
        public string ThreadText { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }


    }
}