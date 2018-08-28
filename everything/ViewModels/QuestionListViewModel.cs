using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace everything.ViewModels
{
    public class QuestionAndReportViewModel
    {
        public int QuestionCount { get; set; }
        public PagedList.IPagedList<QuestionListViewModel> QuestionListViewModels { get; set; }
        public virtual List<TopSevenQuestions> TopSevenQuestions { get; set; }
        public virtual List<TopTenReport> TopTenReports { get; set; }
        public virtual List<MostLikeQuestion> MostLikeQuestions { get; set; }


    }
    public class QuestionListViewModel
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public int AnswerCount { get; set; }
        public string QuestionOwner { get; set; }
        public string DiscussionCategory { get; set; }
        public string DateAsked { get; set; }
        public string LastParticipantDate { get; set; }
        public string LastParticipantOwner { get; set; }

    }
    public class TopSevenQuestions
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionTitle { get; set; }
        public DateTime DateAsked { get; set; }
    }
    public class TopTenReport
    {
        public string Title { get; set; }
        public int ReportId { get; set; }
        public string ReportText { get; set; }
        public DateTime ReportDate { get; set; }
    }

    public class MostLikeQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionText { get; set; }
        public int QuestionLikeCount { get; set; }
        public DateTime DateAsked { get; set; }
    }

}