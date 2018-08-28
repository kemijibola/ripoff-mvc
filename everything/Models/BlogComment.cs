using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class BlogComment
    {
        public int BlogCommentId { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        [Display(Name = "Comment")]
        [AllowHtml]
        [Required(ErrorMessage = "You must enter a comment.")]
        public string Comment { get; set; }

        [Display(Name = "Username")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCommented { get; set; }
    }
}