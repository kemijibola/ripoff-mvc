using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using everything.Models;
using System.Web.Mvc;

namespace everything.ViewModels
{
    public class RebuttalForReportViewModel
    {
        public int ReportId { get; set; }
        public string ReportText { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "You must enter rebuttal report.")]
        public string RebuttalText { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        [Required(ErrorMessage = "You must enter title.")]
        public string Title { get; set; }

    }
    public class UpdateForReportViewModel
    {
        public int ReportId { get; set; }
        public string ReportText { get; set; }
        public string UpdateText { get; set; }

    }
    public class CommentForReportViewModel
    {
        public int ReportId { get; set; }
        public string ReportText { get; set; }
        public string ThreadText { get; set; }

    }
}