using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class RCaseUpdateImage
    {
        public int RCaseUpdateImageId { get; set; }

        [Display(Name = "Case Update Id")]
        [Required(ErrorMessage = "You must enter case id.")]
        public int RipoffCaseUpdateId { get; set; }
        public virtual RipoffCaseUpdate RipoffCaseUpdate { get; set; }

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