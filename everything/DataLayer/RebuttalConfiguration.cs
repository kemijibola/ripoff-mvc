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
    public class RebuttalConfiguration : EntityTypeConfiguration<Rebuttal>
    {
        public RebuttalConfiguration()
        {
            Property(r => r.ReportId)
                .IsRequired();
                

            Property(r => r.UserId)
                .IsRequired();

            Property(r => r.Title)
                .IsRequired();

            Property(r => r.RebuttalText)
                .IsRequired();

            Property(r => r.Address)
                .IsOptional();

            Property(r => r.CityId)
                .IsOptional();

            Property(r => r.StateId)
                .IsOptional();

            Property(r => r.Status)
                .IsOptional();

            Property(r => r.DateCreated)
                .IsRequired();

        }
    }
}