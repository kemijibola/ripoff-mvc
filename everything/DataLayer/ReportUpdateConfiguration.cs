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
    public class ReportUpdateConfiguration : EntityTypeConfiguration<ReportUpdate>
    {
        public ReportUpdateConfiguration()
        {
            Property(ru => ru.ReportId)
                .IsRequired();

            Property(ru => ru.UserId)
                .IsRequired();
            //HasRequired(ru => ru.CurrentUser).WithMany().WillCascadeOnDelete(false);

            Property(ru => ru.UserId)
                .IsRequired();

            Property(ru => ru.Update)
                .IsRequired();

            Property(ru => ru.DateCreated)
                .IsRequired();
        }
    }
}