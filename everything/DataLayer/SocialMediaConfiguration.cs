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
    public class SocialMediaConfiguration : EntityTypeConfiguration<SocialMedia>
    {
        public SocialMediaConfiguration()
        {
            Property(s => s.SocialMediaName)
                .HasMaxLength(30)
                .IsRequired()
                .HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("AK_Social_Media_Social_Media_Name") { IsUnique = true }));

            Property(s => s.SocialMediaHandle)
                .IsRequired();
        }
    }
}