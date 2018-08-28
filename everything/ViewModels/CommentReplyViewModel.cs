using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using everything.Models;

namespace everything.ViewModels
{
    public class CommentReplyViewModel
    {
            public int ReportId { get; set; }
            public string RandomId { get; set; }
            public string PageTitle { get; set; }
            public int ThreadId { get; set; }
            public string ReportText { get; set; }
            public string ThreadText { get; set; }

             [Required]
             public string CommentText { get; set; }
    }
}