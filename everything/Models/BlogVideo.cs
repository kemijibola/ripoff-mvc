using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class BlogVideo
    {
        public int BlogVideoId { get; set; }

        [Display(Name = "Report")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        [Display(Name = "Video Name")]
        [Required(ErrorMessage = "You must enter a video name.")]
        public string VideoName { get; set; }

        [Display(Name = "Video Caption")]
        public string VideoCaption { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}