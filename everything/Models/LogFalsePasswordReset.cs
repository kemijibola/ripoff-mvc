using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class LogFalsePasswordReset
    {
        public int LogFalsePasswordResetId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime DateRequested { get; set; }
        public bool Status { get; set; }
    }
}