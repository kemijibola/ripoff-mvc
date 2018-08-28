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
    public class FirmRegionConfiguration : EntityTypeConfiguration<FirmRegion>
    {
        public FirmRegionConfiguration()
        {
            Property(b => b.Name)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("AK_Region_Name") { IsUnique = true }));
        }
    }
}