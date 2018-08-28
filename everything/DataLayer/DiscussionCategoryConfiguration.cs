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
    public class DiscussionCategoryConfiguration : EntityTypeConfiguration<DiscussionCategory>
    {
        public DiscussionCategoryConfiguration()
        {
            Property(r => r.DiscussionCategoryId)
            .IsRequired();

            Property(r => r.Name)
                .IsRequired();

            Property(r => r.Description)
                .IsRequired();
        }
    }
}