using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace everything.Models
{
    public class Blog
    {
        public Blog()
        {
            BlogImages = new HashSet<BlogImage>();
            BlogComments = new HashSet<BlogComment>();
            BlogVideos = new HashSet<BlogVideo>();

        }
        public int BlogId { get; set; }

        [Display(Name = "Blog Title")]
        [Required(ErrorMessage = "You must enter a blog title.")]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Post { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public bool IsVisible { get; set; }

        public virtual ICollection<BlogImage> BlogImages { get; set; }
        public virtual ICollection<BlogComment> BlogComments { get; set; }
        public virtual ICollection<BlogVideo> BlogVideos { get; set; }
    }
}