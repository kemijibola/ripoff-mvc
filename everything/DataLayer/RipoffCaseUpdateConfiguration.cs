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
    public class RipoffCaseUpdateConfiguration : EntityTypeConfiguration<RipoffCaseUpdate>
    {
        public RipoffCaseUpdateConfiguration()
        {
            Property(cu => cu.ApprovedLawyerRequestId)
                .IsRequired();

            Property(cu => cu.ReportId)
                 .IsRequired();

            Property(cu => cu.UpdateText)
                 .IsRequired();

            Property(cu => cu.AdditionalNote)
                 .IsOptional();

            Property(cu => cu.UpdateDate)
                 .IsRequired();
        }
    }
}