using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class UserProfilePhoto
    {
        public int UserProfilePhotoId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Image Name")]
        public string ImageName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}