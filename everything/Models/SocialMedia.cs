using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class SocialMedia
    {
        public int SocialMediaId { get; set; }
        public string SocialMediaName { get; set; }
        public string  SocialMediaHandle { get; set; }
        
    }
}