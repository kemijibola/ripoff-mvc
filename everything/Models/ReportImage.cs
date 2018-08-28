using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class ReportImage
    {
        public int ReportImageId { get; set; }

        [Display(Name = "Report")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }

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