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
    public class LogFalsePasswordResetConfiguration : EntityTypeConfiguration<LogFalsePasswordReset>
    {
        public LogFalsePasswordResetConfiguration()
        {
            Property(r => r.LogFalsePasswordResetId)
                .IsRequired();

            Property(r => r.UserId)
                .IsRequired();

            Property(r => r.Status)
                .IsRequired();

            Property(r => r.DateRequested)
                .IsRequired();
        }
    }
}