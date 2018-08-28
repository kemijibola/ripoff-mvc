using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace everything.Models
{
    public class QuestionLike
    {
        public int QuestionLikeId { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public string IPAddress { get; set; }
        public bool UserLike { get; set; }        

        public virtual Question Question { get; set; }
    }
}