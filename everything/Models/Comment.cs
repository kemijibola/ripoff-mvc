using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [Display(Name = "Thread")]
        public int ThreadId { get; set; }
        public virtual Thread Thread { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Reply")]
        [Required(ErrorMessage = "You must enter a comment.")]
        public string CommentText { get; set; }


        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}