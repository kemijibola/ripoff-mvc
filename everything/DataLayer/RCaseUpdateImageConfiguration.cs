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
    public class RCaseUpdateImageConfiguration : EntityTypeConfiguration<RCaseUpdateImage>
    {
        public RCaseUpdateImageConfiguration()
        {
            Property(rci => rci.RipoffCaseUpdateId)
                 .IsRequired();
            Property(rci => rci.ImageName)
                 .IsRequired();
            Property(rci => rci.ImageCaption)
                    .IsOptional();
            Property(rci => rci.DateCreated)
                 .IsRequired();
        }
    }
}