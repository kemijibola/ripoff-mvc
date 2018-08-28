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
    public class UpdateAdviceConfiguration : EntityTypeConfiguration<UpdateAdvice>
    {
        public UpdateAdviceConfiguration()
        {

            Property(t => t.ReportUpdateId)
                 .IsRequired();


            Property(t => t.UserId)
                .IsRequired();

            Property(t => t.AdviseText)
                .IsRequired();

            Property(t => t.DateCreated)
                .IsRequired();
        }
    }
}