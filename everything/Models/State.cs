using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class State
    {
        public int StateId { get; set; }

        [Display(Name = "City Name")]
        [Required(ErrorMessage = "You must enter a city name.")]
        public string Name { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "You must enter country id.")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}