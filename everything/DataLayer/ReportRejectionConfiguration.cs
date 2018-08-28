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
    public class ReportRejectionConfiguration : EntityTypeConfiguration<ReportRejection>
    {
        public ReportRejectionConfiguration()
        {
            Property(rr => rr.ReportId)
                .IsRequired();

            Property(rr => rr.RejectionReasonId)
                .IsRequired();

            Property(rr => rr.DateCreated)
                .IsRequired();
        }
    }
}