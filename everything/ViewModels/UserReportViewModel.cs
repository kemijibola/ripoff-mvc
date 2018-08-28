using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using everything.Models;

namespace everything.ViewModels
{
    public class UserReportViewModel
    {
        public string userId { get; set; }
        public virtual List<TopTenQuestionForLatestReportViewModel> TopTenQuestionForLatestReportViewModels { get; set; }
        public PagedList.IPagedList<ReportsWithOwner> ReportsWithOwners { get; set; }

    }

    public class FullNewsViewModel
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string PublishedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string CommentBy { get; set; }
        public virtual IEnumerable<BlogImage> BlogImages { get; set; }
        public virtual IEnumerable<BlogVideo> BlogVideos { get; set; }
        public virtual IEnumerable<BlogComment> BlogComment { get; set; }
        public virtual List<TopSevemNews> TopSevemNews { get; set; }
    }


    public class NewsCreatedByViewModel
    { 
        public PagedList.IPagedList<AllNewsViewModel> NewsWithOwner { get; set; }
    }

    public class TopSevemNews
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class AllNewsViewModel
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Post { get; set; }
        public string PostedBy { get; set; }
        public DateTime Created { get; set; }
    }
    public class TopTenQuestionForLatestReportViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionOwner { get; set; }
        public string QuestionText { get; set; }
        public string QuestionTitle { get; set; }
        public string DateAsked { get; set; }
        public string TimeAsked { get; set; }
    }


}