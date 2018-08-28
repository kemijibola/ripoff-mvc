using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace everything.Models
{
    public class Report
    {
        public int ReportId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        

        [Display(Name = "Company or Individual")]
        [Required(ErrorMessage = "Name is required.")]
        public string CompanyorIndividual { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Topic")]
        [Required(ErrorMessage = "You must enter topic.")]
        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "You must enter category.")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }


        [Required(ErrorMessage ="You must enter a title ")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You must write a detailed report.")]
        [AllowHtml]
        public string ReportText { get; set; }

        public bool OnlineTransaction { get; set; }
        public bool CreditCard { get; set; }

        [Required]
        public bool Status { get; set; }

        public bool RejectionStatus { get; set; }

        [Required]
        public bool AdviceStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        [Display(Name = "State")]
        public int? StateId { get; set; }
        public virtual State State { get; set; }


        [Display(Name = "Phone Number")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "The Phone number is not valid.")]
        public string ContactNumber { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "The Email is not valid.")]
        public string Email { get; set; }

        //ReportImageViewModel

        //[Display(Name = "Image Caption")]
        //public string ImageCaption { get; set; }


        public virtual ICollection<ReportImage> ReportImages { get; set; }

        public virtual ICollection<ReportVideo> ReportVideos { get; set; }
        public virtual ICollection<Rebuttal> Rebuttals { get; set; }
        public virtual ICollection<ReportUpdate> ReportUpdates { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<ClientLawsuit> ClientLawsuits { get; set; }
        public virtual ICollection<RipoffCaseUpdate> RipoffCaseUpdates { get; set; }
        public virtual ICollection<LawyerRequest> LawyerRequests { get; set; }
        public virtual ICollection<RipOffLegalTeamAdvice> RipOffLegalTeamAdvices { get; set; }
    }

}