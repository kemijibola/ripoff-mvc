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
    public class ReportConfiguration : EntityTypeConfiguration<Report>
    {
        public ReportConfiguration()
        {
            Property(rpt => rpt.UserId)
                .IsRequired();
            // HasRequired(rpt => rpt.CurrentUser).WithMany().WillCascadeOnDelete(false);

            Property(rpt => rpt.CompanyorIndividual)
                .IsRequired();

            Property(rpt => rpt.Website)
                .IsOptional();

            Property(rpt => rpt.TopicId)
                .IsRequired();

            Property(rpt => rpt.CategoryId)
                .IsRequired();

            Property(rpt => rpt.ReportText)
                .IsRequired();

            Property(rpt => rpt.OnlineTransaction)
                .IsOptional();

            Property(rpt => rpt.CreditCard)
                .IsOptional();

            Property(rpt => rpt.Status)
                .IsRequired();

            Property(rpt => rpt.RejectionStatus)
                .IsOptional();

            Property(rpt => rpt.DateCreated)
                .IsRequired();

            Property(rpt => rpt.Address)
                .IsOptional();

            Property(cy => cy.CityId).IsOptional();

            Property(st => st.StateId).IsOptional();

            Property(rpt => rpt.ContactNumber)
                .IsOptional()
                .HasMaxLength(11);


            Property(rpt => rpt.Email)
                .IsOptional();

        }
    }
}