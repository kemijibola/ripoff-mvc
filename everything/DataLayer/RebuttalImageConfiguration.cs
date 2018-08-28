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
    public class RebuttalImageConfiguration : EntityTypeConfiguration<RebuttalImage>
    {
        public RebuttalImageConfiguration()
        {
            Property(ri => ri.RebuttalId)
                .IsRequired();

            Property(ri => ri.ImageName)
                .IsRequired();

            Property(ri => ri.ImageCaption)
                .IsRequired();

            Property(ri => ri.DateCreated)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .IsRequired();
        }
    }
}