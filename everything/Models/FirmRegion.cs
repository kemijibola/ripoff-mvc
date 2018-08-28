using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class FirmRegion
    {
        public int FirmRegionId { get; set; }

        [Display(Name = "Region Name")]
        [Required(ErrorMessage = "You must enter a region name.")]
        [StringLength(20, ErrorMessage = "The region name must be 20 characters or shorter.")]
        public string Name { get; set; }
    }
}