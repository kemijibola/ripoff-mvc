using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class Topic
    {
        public int TopicId { get; set; }

        [Display(Name = "Topic Name")]
        [Required(ErrorMessage = "You must enter a topic name.")]
        [StringLength(70, ErrorMessage = "The topic name must be 70 characters or shorter.")]
        public string Name { get; set; }
    }
}