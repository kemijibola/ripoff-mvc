using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace everything.Models
{
    public class RebuttalImage
    {
        public int RebuttalImageId { get; set; }

        [Display(Name = "Rebuttal")]
        [Required(ErrorMessage = "You must enter rebuttal id.")]
        public int RebuttalId { get; set; }
        public virtual Rebuttal Rebuttal { get; set; }

        [Display(Name = "Image Name")]
        [Required(ErrorMessage = "You must enter a image name.")]
        public string ImageName { get; set; }

        [Display(Name = "Image Caption")]
        [Required(ErrorMessage = "You must enter a image caption.")]
        public string ImageCaption { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

    }
}