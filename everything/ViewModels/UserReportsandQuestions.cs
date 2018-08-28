using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace everything.ViewModels
{
    public class UserReportsandQuestionsWithOwner
    {
        public int UserId { get; set; }
        public PagedList.IPagedList<UserReport> UserReports { get; set; }
        public PagedList.IPagedList<UserQuestion> UserQuestions { get; set; }

    }

    public class NewsCommentAndEmail
    {
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
    }
    public class UserReport
    {
        public int ReportId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public bool hasRequestedLegalService { get; set; }
        public DateTime DateCreated { get; set; }


    }
    public class UserQuestion
    {
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public bool isAnswered { get; set; }
        public DateTime DateAsked { get; set; }
    }
}