using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class DiscussionCategory
    {
        public int DiscussionCategoryId { get; set; }

        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "You must enter a discussion category name.")]
        [StringLength(70, ErrorMessage = "The discussion name must be 70 characters or shorter.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must description.")]
        public string Description { get; set; }
    }
}