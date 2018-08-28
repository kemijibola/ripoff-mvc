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
    public class RipOffLegalTeamAdviceConfiguration : EntityTypeConfiguration<RipOffLegalTeamAdvice>
    {
        public RipOffLegalTeamAdviceConfiguration()
        {
            Property(lt => lt.ReportId)
                .IsRequired();
            Property(lt => lt.UserId)
                .IsRequired();
            Property(lt => lt.LegalAdvice)
                  .IsRequired();
            Property(lt => lt.DateCreated)
                .IsRequired();
        }
    }
}