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
    public class QuestionConfiguration : EntityTypeConfiguration<Question>
    {
        public QuestionConfiguration()
        {
            Property(r => r.QuestionId)
                .IsRequired();


            Property(r => r.UserId)
                .IsRequired();

            Property(r => r.Title)
                .IsRequired();

            Property(r => r.QuestionText)
                .IsRequired();

            Property(r => r.Tag)
                .IsRequired();

            Property(r => r.DiscussionCategoryId)
                .IsRequired();

            Property(r => r.VoteScore)
                .IsOptional();

            Property(r => r.isAnswered)
                .IsOptional();

            Property(r => r.NotifyMe)
                .IsOptional();

            Property(r => r.DateAsked)
                .IsRequired();
        }
        
    }
}