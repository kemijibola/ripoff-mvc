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
    public class RejectedLawyerRequestConfiguration : EntityTypeConfiguration<RejectedLawyerRequest>
    {
        public RejectedLawyerRequestConfiguration()
        {
            Property(rlr => rlr.LawyerRequestId)
                .IsRequired();

            Property(rlr => rlr.LawyerRejectionReasonId)
                 .IsRequired();

            Property(rlr => rlr.DateRejected)
                 .IsRequired();
        }
    }
}