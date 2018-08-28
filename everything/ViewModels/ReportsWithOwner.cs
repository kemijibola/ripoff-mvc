using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using everything.Models;

namespace everything.ViewModels
{
    public class ReportsWithOwner
    {
        public string RandomId { get; set; }
        public int ReportId { get; set; }
        public string PageTitle { get; set; }
        public string CompanyorIndividual { get; set; }
        public string ReportText { get; set; }
        public string DisplayName { get; set; }
        public string CategoryName { get; set; }
        public string TopicName { get; set; }
        public bool ReportImagesExist { get; set; }
        public int ReportImagesCount { get; set; }
        public bool ReportVideosExist { get; set; }
        public int ReportVideoCount { get; set; }
        public bool ReportUpdateExist { get; set; }
        public int ReportUpdateCount { get; set; }
        public bool RebuttalsExist { get; set; }
        public int RebuttalsCount { get; set; }
        public bool ThreadsExist { get; set; }
        public int ThreadsCount { get; set; }
        public bool CaseUpdatesExist { get; set; }
        public int CaseUpdatesCount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}