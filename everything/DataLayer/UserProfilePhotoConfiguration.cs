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
    public class UserProfilePhotoConfiguration : EntityTypeConfiguration<UserProfilePhoto>
    {
        public UserProfilePhotoConfiguration()
        {
            Property(r => r.UserProfilePhotoId)
            .IsRequired();

            Property(r => r.UserId)
                .IsRequired();

            Property(r => r.ImageName)
                .IsRequired();

            Property(r => r.DateCreated)
                .IsRequired();
        }
    }
}