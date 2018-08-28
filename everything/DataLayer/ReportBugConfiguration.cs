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
    public class ReportBugConfiguration : EntityTypeConfiguration<ReportBug>
    {
        public ReportBugConfiguration()
        {
            Property(rb => rb.UserId)
                .IsRequired();
            //HasRequired(rb => rb.CurrentUser).WithMany().WillCascadeOnDelete(false);

            Property(rb => rb.Error)
                .IsRequired()
                .HasMaxLength(250);

            Property(rb => rb.DateCreated)
                .IsRequired();
        }
    }
}