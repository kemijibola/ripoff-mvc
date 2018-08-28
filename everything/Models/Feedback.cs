using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Feedback Message")]
        [Required(ErrorMessage = "You must enter feedback message.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}