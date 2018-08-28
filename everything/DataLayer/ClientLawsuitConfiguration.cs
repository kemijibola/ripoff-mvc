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
    public class ClientLawsuitConfiguration : EntityTypeConfiguration<ClientLawsuit>
    {
        public ClientLawsuitConfiguration()
        {

            Property(cl => cl.UserId)
                .IsRequired();
            //HasRequired(cl => cl.CurrentUser).WithMany().WillCascadeOnDelete(false);

            Property(cl => cl.ReportId)
                .IsRequired();

            Property(cl => cl.LawfirmId)
                .IsRequired();

            Property(cl => cl.StartCreated)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .IsRequired();

            Property(cl => cl.EndCreated)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .IsRequired();

            Property(cl => cl.Resolution)
                .HasMaxLength(250)
                .IsRequired();
        }

    }
}