﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using everything.Models;

namespace everything.DataLayer
{
    public class AnswerCommentConfiguration : EntityTypeConfiguration<AnswerComment>
    {
        public AnswerCommentConfiguration()
        {
            Property(r => r.AnswerCommentId)
            .IsRequired();

            Property(r => r.UserId)
                .IsRequired();

            Property(r => r.AnswerToQuestionId)
                .IsRequired();

            Property(r => r.Comment)
                .IsRequired();


            Property(r => r.DateCommented)
                .IsRequired();
        }
    }
}