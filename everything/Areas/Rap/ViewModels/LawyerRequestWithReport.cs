using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace everything.Areas.Rap.ViewModels
{
    public class LawyerRequestWithReportViewModel
    {
        public string ReportTitle { get; set; }
        public PagedList.IPagedList<LawyerRequestViewModel> LawyerRequestViewModels { get; set; }
    }
    public class LawyerRequestViewModel
    {
        public int Id { get; set; }
        public string RequestOwnerName { get; set; }
        public int Status { get; set; }
    }
}