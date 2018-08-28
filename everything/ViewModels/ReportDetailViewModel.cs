using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using everything.Models;

namespace everything.ViewModels
{
    public class FullReportViewModel
    {
        public int ReportId { get; set; }
        public string DisplayName { get; set; }
        public string ImageName { get; set; }
        public string CompanyorIndividual { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }

        public string CategoryName { get; set; }
        public string TopicName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string ReportText { get; set; }
        public string Title { get; set; }
        public bool OnlineTransaction { get; set; }
        public bool CreditCard { get; set; }
        public bool CaseUpdateExist { get; set; }
        public bool LegalAdviceExist { get; set; }
        public bool RebuttalAccess { get; set; }
        public string ReportTimeCreated { get; set; }
        public string ReportDateCreated { get; set; }
        public virtual List<ThreadViewModel> ThreadViewModels { get; set; }
        public virtual List<RebuttalViewModel> RebuttalViewModels { get; set; }
        public virtual List<ReportUpdateViewModel> ReportUpdateViewModels { get; set; }
        public virtual List<RipOffCaseUpdateViewModel> RipOffCaseUpdateViewModels { get; set; }
        public virtual List<ReportImageViewModel> ReportImageViewModels { get; set; }
        public virtual List<ReportVideoViewModel> ReportVideoViewModels { get; set; }
        public virtual List<CommentViewModel> CommentViewModels { get; set; }
        public virtual List<RipOffLegalTeamViewModel> RipOffLegalTeamViewModels { get; set; }
        public virtual List<RelatedReportedCompanyViewModel> RelatedReportedCompanyViewModels { get; set; }
        public virtual List<LegalAdviceViewModel> LegalAdviceViewModels { get; set; }

    }
    public class RelatedReportedCompanyViewModel
    {
        public int ReportId { get; set; }
        public string ReportText { get; set; }

    }
    public class RebuttalViewModel
    {
            public string RebuttalDisplayName { get; set; }
            public string ReportText { get; set; }
            public string RebuttalTitle { get; set; }
            public string RebuttalText { get; set; }
            public string RebuttalAddress { get; set; }
            public string RebuttalState { get; set; }
            public string RebuttalCity { get; set; }
            public string RebuttalTimeCreated { get; set; }
            public string RebuttalDateCreated { get; set; }
         
     }

    public class RipOffLegalTeamViewModel
    {
       
        public int ReportId { get; set; }
        public string LegalAdvice { get; set; }
        public DateTime DateCreated { get; set; }

    }

    public class LegalAdviceViewModel
    {
        public int ReportUpdateId { get; set; }
        public string UpdateAdvice { get; set; }

    }
    public class ThreadViewModel
    {

        public int ThreadId { get; set; }
        public string ThreadDisplayName { get; set; }
        public string ThreadImageName { get; set; }
        public string ThreadText { get; set; }
        public string ThreadTimeCreated { get; set; }
        public string ThreadDateCreated { get; set; }
    }
    public class CommentViewModel
    {
        public int ThreadId { get; set; }
        public string CommentText { get; set; }
        public string CommentDisplayName { get; set; }
        public string CommentImageName { get; set; }
        public string CommentTimeCreated { get; set; }
        public string CommentDateCreated { get; set; }
    }

    public class ReportUpdateViewModel
    {
        public int ReportUpdateId { get; set; }
        public string ReportUpdateDisplayName { get; set; }
        public string ReportUpdateText { get; set; }
        public string ReportUpdateTimeCreated { get; set; }
        public string ReportUpdateDateCreated { get; set; }
    }
    public class ReportImageViewModel
    {
        public string ImageName { get; set; }
        public string ImageCaption { get; set; }
        public DateTime ImageDateCreated { get; set; }
    }
    public class ReportVideoViewModel
    {
        public string VideoName { get; set; }
        public string VideoCaption { get; set; }
        public DateTime VideoDateCreated { get; set; }
    }
    public class RipOffCaseUpdateViewModel
    {
        public string CaseUpdateText { get; set; }
    }
}