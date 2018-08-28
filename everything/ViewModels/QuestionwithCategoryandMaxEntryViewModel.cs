using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using everything.Models;
using System.Web.Mvc;

namespace everything.ViewModels
{
    public class QuestionwithCategoryandMaxEntryViewModel
    {
        public virtual List<DiscussionModel> DiscussionModels { get; set; }
    }
    public class DiscussionModel
    {
        public int DiscussionCategoryId { get; set; }
        public string Description { get; set; }
        public string DiscussionName { get; set; }
        public int TotalEntryForCategory { get; set; }
    }
}