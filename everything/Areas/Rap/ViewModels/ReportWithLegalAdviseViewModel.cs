using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace everything.Areas.Rap.ViewModels
{
    public class ReportWithLegalAdviseViewModel
    {
        public virtual List<ReportListView> ReportListViews { get; set; }
        public virtual List<ReportUpdateView> ReportUpdateViews { get; set; }
    }

    public class ReportUpdateView
    {

        public int ReportId { get; set; }
        public string UpdateText { get; set; }
        public DateTime DatePosted { get; set; }
    }

    public class ReportListView
    {
        public int ReportId { get; set; }
        public string ReportTitle { get; set; }
        public string ReportText { get; set; }
    }

}