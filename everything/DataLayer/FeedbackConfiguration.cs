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
    public class FeedbackConfiguration : EntityTypeConfiguration<Feedback>
    {
        public FeedbackConfiguration()
        {
            Property(fb => fb.UserId)
                .IsRequired();
            //HasRequired(fb => fb.CurrentUser).WithMany().WillCascadeOnDelete(false);

            Property(fb => fb.Message)
                .IsRequired();

            Property(fb => fb.DateCreated)
                .IsRequired();
        }
    }
}