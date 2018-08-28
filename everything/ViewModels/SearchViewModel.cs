using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using everything.Models;
namespace everything.ViewModels
{
    public class SearchViewModel
    {
        public string Keyword { get; set; }

        public PagedList.IPagedList<ReportSearch> ReportSearchs { get; set; }
    }
    public class KeyViewModel
    {
        public string Keyword { get; set; }

    }
    public class SearchResultViewModel
    {
        public int page { get; set; }
        public string title { get; set; }
        public string iD { get; set; }
        public string company { get; set; }
        public string Topic { get; set; }
        public string Category { get; set; }

    }
    public class ReportSearch
    {
        public string ReportOwnwer { get; set; }
        public int ReportId { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }

        public string CategoryName { get; set; }

        public string TopicName { get; set; }

        public string CompanyorIndividual { get; set; }

        public string ReportText { get; set; }


        public DateTime DateCreated { get; set; }



    }
}