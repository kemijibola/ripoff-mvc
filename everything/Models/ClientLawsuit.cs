using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class ClientLawsuit
    {
        public int ClientLawsuitId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        

        [Display(Name = "Report")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int ReportId { get; set; }
        public virtual Report Report { get; set; }

        [Display(Name = "Lawfirm")]
        [Required(ErrorMessage = "You must enter report id.")]
        public int LawfirmId { get; set; }
        public virtual Lawfirm Lawfirm { get; set; }

        [Required(ErrorMessage = "You must enter start date.")]
        [DataType(DataType.Date)]
        public DateTime StartCreated { get; set; }

        [Required(ErrorMessage = "You must enter end date.")]
        [DataType(DataType.Date)]
        public DateTime EndCreated { get; set; }

        [Required(ErrorMessage = "You must enter resolution.")]
        public string Resolution { get; set; }
    }
}