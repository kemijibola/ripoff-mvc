using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class Bank
    {
        public int BankId { get; set; }

        [Display(Name = "Bank Name")]
        [Required(ErrorMessage = "You must enter a bank name.")]
        public string Name { get; set; }

    }
}