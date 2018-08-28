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
    public class LawyerRequestConfiguration : EntityTypeConfiguration<LawyerRequest>
    {
        public LawyerRequestConfiguration()
        {
            Property(lr => lr.UserId )
                .IsRequired();

            Property(lr => lr.FirmRegionId)
                 .IsRequired();

            Property(lr => lr.ReportId)
                 .IsRequired();

            Property(lr => lr.Email)
                 .IsRequired();

            Property(lr => lr.PhoneNumber)
                 .IsRequired();

            Property(lr => lr.AdditionalNote)
                 .IsOptional();

            Property(lr => lr.AssignedToFirm)
                 .IsRequired();

            Property(lr => lr.RequestDate)
                .IsRequired();
        }
    }
}