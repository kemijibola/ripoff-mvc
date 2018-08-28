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
    public class QuestionVoteConfiguration : EntityTypeConfiguration<QuestionVote>
    {
        public QuestionVoteConfiguration()
        {
            Property(b => b.QuestionId)
                .IsRequired();

            Property(b => b.UserId)
                .IsRequired();

            Property(b => b.HasVoted)
                .IsRequired();

        }
    }
}