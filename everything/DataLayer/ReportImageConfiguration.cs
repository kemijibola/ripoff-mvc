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
    public class ReportImageConfiguration : EntityTypeConfiguration<ReportImage>
    {
        public ReportImageConfiguration()
        {
            Property(rpti => rpti.ReportId)
                .IsRequired();

            Property(rpti => rpti.ImageName)
                .IsRequired();

            Property(rpti => rpti.ImageCaption)
                .IsOptional();

            Property(rpti => rpti.DateCreated)
                .IsRequired();
        }
    }
}