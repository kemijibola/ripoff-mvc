using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class QuestionVote
    {
        public int QuestionVoteId { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        [Required]
        public bool HasVoted { get; set; }
    }
}