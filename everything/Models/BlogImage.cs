using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace everything.Models
{
    public class BlogImage
    {
        public int BlogImageId { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        [Display(Name = "Image Name")]
        [Required(ErrorMessage = "You must upload minimum 1 image")]
        public string ImageName { get; set; }

        [Display(Name = "Image Caption")]
        public string ImageCaption { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}