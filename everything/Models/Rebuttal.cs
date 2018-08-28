using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class Rebuttal
    {
        public int RebuttalId { get; set; }

        [Display(Name = "Report")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "You must enter a title.")]
        public string Title { get; set; }

        [Display(Name = "Rebuttal Text")]
        [Required(ErrorMessage = "You must enter a rebuttal text.")]
        public string RebuttalText { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        [Display(Name = "State")]
        public int? StateId { get; set; }
        public virtual State State { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "You must enter date created.")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }


    }
}