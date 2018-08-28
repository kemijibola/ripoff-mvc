using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class Question
    {
        public Question()
        {
            this.QuestionLikes = new HashSet<QuestionLike>();
        }
        public int QuestionId { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "You must enter a title.")]
        public string Title { get; set; }

        [AllowHtml]
        [Display(Name = "Question Text")]
        [Required(ErrorMessage = "You must enter question text.")]
        public string QuestionText { get; set; }

        [Display(Name = "Tag")]
        [Required(ErrorMessage = "You must enter tag.")]
        public string Tag { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get;  set; }

        [Display(Name = "Discussion Category")]
        [Required]
        public int DiscussionCategoryId { get; set; }
        public virtual DiscussionCategory DiscussionCategory { get; set; }

        public int? VoteScore { get; set; }

        public bool isAnswered { get; set; }

        public bool NotifyMe { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAsked { get; set; }

        public virtual ICollection<QuestionComment> QuestionComments { get; set; }
        public virtual ICollection<AnswerToQuestion> AnswerToQuestions { get; set; }
        public virtual ICollection<QuestionLike> QuestionLikes { get; set; }

    }
}