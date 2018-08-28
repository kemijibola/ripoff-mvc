using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace everything.Models
{
    public class CustomEmailValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string email = value.ToString();

                if (Regex.IsMatch(email, @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", RegexOptions.IgnoreCase))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Valid email required.");
                }
            }
            else
            {
                return new ValidationResult("" + validationContext.DisplayName + " is required");
            }
        }
    }

    public class CustomCompanyNameValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string name = value.ToString();

                if (name.Length > 1 && name.Length <= 50)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Name too short");
                }
            }
            else
            {
                return new ValidationResult("" + validationContext.DisplayName + " is required");
            }
        }
    }

    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int maxContent = 1024 * 1024; //1 MB
            string[] sAllowedExt = new string[] { ".jpg", ".gif", ".png" };


            var file = value as HttpPostedFileBase;

            if (file == null)
                return false;
            else if (!sAllowedExt.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Please upload Your Photo of type: " + string.Join(", ", sAllowedExt);
                return false;
            }
            else if (file.ContentLength > maxContent)
            {
                ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (maxContent / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }
    }
}