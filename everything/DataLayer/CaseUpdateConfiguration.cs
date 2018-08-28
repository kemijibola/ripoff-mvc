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
    public class CaseUpdateConfiguration : EntityTypeConfiguration<CaseUpdate>
    {
        public CaseUpdateConfiguration()
        {
            Property(cu => cu.ReportId)
                .IsRequired();

            Property(cu => cu.LawfirmId)
                .IsRequired();

            Property(cu => cu.Update)
                .IsRequired();

            Property(cu => cu.Status)
                .IsRequired();

            Property(cu => cu.DateCreated)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .IsRequired();
        }

    }
}