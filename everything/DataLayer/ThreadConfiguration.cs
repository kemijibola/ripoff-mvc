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
    public class ThreadConfiguration : EntityTypeConfiguration<Thread>
    {
        public ThreadConfiguration()
        {
            Property(t => t.ReportId)
                .IsRequired();

            Property(t => t.UserId)
                .IsRequired();
            //HasRequired(t => t.CurrentUser).WithMany().WillCascadeOnDelete(false);

            Property(t => t.ThreadText)
                .IsRequired();

            Property(t => t.DateCreated)
                .IsRequired();
        }
    }
}