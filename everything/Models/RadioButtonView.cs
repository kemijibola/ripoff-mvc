using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace everything.Models
{
    public class RadioButtonView
    {

        [Display(Name = "Selection")]
        public string Selection { get; set; }

        public RadioButtonView()
        {
            Selection = "Image";
        }
    }
}