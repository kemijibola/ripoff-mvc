using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class AnswerToQuestion
    {
        public int AnswerToQuestionId { get; set; }

        [Display(Name = "Question")]
        [Required(ErrorMessage = "You must enter question.")]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Comment")]
        [Required(ErrorMessage = "You must enter answer.")]
        public string Answer { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAnswered { get; set; }

        public virtual ICollection<AnswerComment> AnswerComments { get; set; }
    }
}