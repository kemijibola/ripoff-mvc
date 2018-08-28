using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class Country
    {
        public int CountryId { get; set; }

        [Display(Name = "Country Name")]
        [Required(ErrorMessage = "You must enter a country name.")]
        public string Name { get; set; }
    }
}