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
    public class LawfirmConfiguration : EntityTypeConfiguration<Lawfirm>
    {
        public LawfirmConfiguration()
        {
            Property(lf => lf.FirmName)
                .HasMaxLength(30)
                .IsRequired();

            Property(lf => lf.HolderName)
                .HasMaxLength(30)
                .IsRequired();

            Property(lf => lf.PhoneNumber)
                .HasMaxLength(11)
                .IsRequired();

            Property(lf => lf.ContactPerson)
                .HasMaxLength(30)
                .IsRequired();

            Property(lf => lf.ContactNumber)
                .HasMaxLength(11)
                .IsRequired();

            Property(lf => lf.Address)
                .HasMaxLength(50)
                .IsRequired();

            Property(lf => lf.Email)
                .IsOptional();

            Property(lf => lf.CityId)
                .IsRequired();

            Property(lf => lf.StateId)
                .IsRequired();

            Property(lf => lf.CountryId)
                .IsOptional();

            Property(lf => lf.DateRegistered)
                .IsRequired();

        }
    }
}