using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class QuestionComment
    {
        public int QuestionCommentId { get; set; }

        [Display(Name = "Question")]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        [Display(Name = "Comment")]
        [Required(ErrorMessage = "You must enter a comment.")]
        public string Comment { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCommented { get; set; }


    }
}