using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace everything.Models
{
    public class LikeQuestion
    {
        public int LikeCount { get; set; }
        public bool UserLikeStatus { get; set; }
    }
}