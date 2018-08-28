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
    public class AnswerToQuestionConfiguration : EntityTypeConfiguration<AnswerToQuestion>
    {
        public AnswerToQuestionConfiguration()
        {
            Property(r => r.AnswerToQuestionId)
                .IsRequired();

            Property(r => r.QuestionId)
                .IsRequired();

            Property(r => r.AnswerToQuestionId)
                .IsRequired();

            Property(r => r.UserId)
                .IsRequired();

            Property(r => r.Answer)
                 .IsRequired();

            Property(r => r.DateAnswered)
                .IsRequired();
        }
    }
}