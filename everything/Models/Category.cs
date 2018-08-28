using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace everything.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Display(Name = "Category Name")]
        [Required (ErrorMessage = "You must enter category name.")]
        [StringLength(50, ErrorMessage = "The category name must be 50 characters or shorter.")]
        public string Name { get; set; }

        [Display(Name = "Topic")]
        [Required(ErrorMessage = "You must enter topic.")]
        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }
    }
}