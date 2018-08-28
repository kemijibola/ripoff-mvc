using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using everything.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace everything.DataLayer
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            Property(au => au.FirstName).HasMaxLength(15).IsOptional();
            Property(au => au.NameExtension).HasMaxLength(50).IsRequired();
            Property(au => au.LastName).HasMaxLength(15).IsOptional();
            Property(au => au.Address).HasMaxLength(50).IsOptional();
            Property(au => au.CityId).IsOptional();
            Property(au => au.StateId).IsOptional();
            Property(au => au.CountryId).IsOptional();
            Property(au => au.IsInterestedInLawyer).IsOptional();
            Property(au => au.DisableRP).IsOptional();
            Property(au => au.Career).IsRequired();
            Property(au => au.DateRegistered)
                .IsRequired();
        }
    }
}