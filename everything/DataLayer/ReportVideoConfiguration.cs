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
    public class ReportVideoConfiguration : EntityTypeConfiguration<ReportVideo>
    {
        public ReportVideoConfiguration()
        {
            Property(rpti => rpti.ReportId)
                .IsRequired();

            Property(rpti => rpti.VideoName)
                .IsRequired();

            Property(rpti => rpti.VideoCaption)
                .IsOptional();

            Property(rpti => rpti.DateCreated)
                .IsRequired();
        }
    }
}