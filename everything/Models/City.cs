using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace everything.Models
{
    public class City
    {
        public int CityId { get; set; }

        [Display(Name = "City Name")]
        [Required(ErrorMessage = "You must enter a city name.")]
        public string Name { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "You must enter state id.")]
        public int StateId { get; set; }
        public virtual State State { get; set; }

    }
}