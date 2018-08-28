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
    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {

            Property(t => t.ThreadId)
                 .IsRequired();
                 

            Property(t => t.UserId)
                .IsRequired();
            //HasRequired(t => t.CurrentUser).WithMany().WillCascadeOnDelete(false);

            Property(t => t.CommentText)
                .IsRequired();

            Property(t => t.DateCreated)
                .IsRequired();
        }
    }
}