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
    public class ClientMeetingRequestTempConfiguration : EntityTypeConfiguration<ClientMeetingRequestTemp>
    {
        public ClientMeetingRequestTempConfiguration()
        {
            Property(cm => cm.UserId)
                .IsRequired();
            //HasRequired(cl => cl.CurrentUser).WithMany().WillCascadeOnDelete(false);

            Property(cm => cm.FirmRegionId)
                .IsRequired();

            Property(cm => cm.ReportId)
                .IsRequired();

            Property(cm => cm.Email)
                .IsOptional();

            Property(cm => cm.PhoneNumber)
                .HasMaxLength(11)
                .IsRequired();

            Property(cm => cm.AssignedToFirm)
                .IsRequired();

            Property(cm => cm.InitiateCreated)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .IsRequired();

        }
    }
}