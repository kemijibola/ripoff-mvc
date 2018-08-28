using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace everything.Areas.Rap.ViewModels
{
    public class LawFirmViewModel
    {
        public string FirmName { get; set; }
        public string HolderName { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

    }

    public class AdminInterfaceViewModel
    {
        public string SelectedRole { get; set; }
    }
}