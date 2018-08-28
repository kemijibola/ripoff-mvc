using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using everything.Models;

namespace everything.ViewModels
{
    
    public class FeedbackWithBlog
    {
        public virtual List<BlogViewModel> Posts { get; set; }
        public virtual List<FeedbackViewModel> Testimonials { get; set; }
    }
    public class FeedbackViewModel
    {
        public string UserPicture { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Message { get; set; }
    }
    public class BlogViewModel 
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class IndexReportViewModel
    {
        public int ReportId { get; set; }
        public string DisplayName { get; set; }
        public string CompanyorIndividual { get; set; }
        public string ReportText { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class IndexQuestionViewModel
    {
        public int QuestionId { get; set; }
        public string DisplayName { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionText { get; set; }
        public DateTime DateAsked { get; set; }
    }
    public class HomeViewModel 
    {
        [Required]
        public string Keyword { get; set; }

        public virtual List<IndexReportViewModel> IndexReportViewModels { get; set; }
        public virtual List<IndexQuestionViewModel> IndexQuestionViewModels { get; set; }
    }
}