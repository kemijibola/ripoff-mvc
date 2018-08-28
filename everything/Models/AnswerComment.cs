using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class AnswerComment
    {
        public int AnswerCommentId { get; set; }

        [Display(Name = "Answer")]
        public int AnswerToQuestionId { get; set; }
        public virtual AnswerToQuestion AnswerToQuestion { get; set; }

        [Display(Name = "Comment")]
        [AllowHtml]
        [Required(ErrorMessage = "You must enter a comment.")]
        public string Comment { get; set; }

        [Display(Name = "Username")]
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCommented { get; set; }

    }
}