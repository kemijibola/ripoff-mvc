using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using everything.Models;

namespace everything.DataLayer
{
    public class QuestionLikeConfiguration : EntityTypeConfiguration<QuestionLike>
    {
        public QuestionLikeConfiguration()
        {
            Property(t => t.QuestionId)
            .IsRequired();

            Property(t => t.UserAgent)
                .IsRequired();

            Property(t => t.UserId)
                .IsRequired();

            Property(t => t.UserLike)
                .IsRequired();
        }
    }
}