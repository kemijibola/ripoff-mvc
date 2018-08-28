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
    public class TopicConfiguration : EntityTypeConfiguration<Topic>
    {
        public TopicConfiguration()
        {
            Property(tp => tp.Name)
                .IsRequired()
                .HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("AK_Topic_Name") { IsUnique = true }));
        }
    }
}