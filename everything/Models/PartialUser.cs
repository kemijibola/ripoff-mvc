using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace everything.Models {
    [MetadataType(typeof(UserMetaData))]

    public partial class RegisterViewModel
    {

    }

    class UserMetaData {
        [Remote("NameAlreadyExistsAsync", "Account", ErrorMessage = "User name already in use.")]
        public string UserName { get; set; }

        [Remote("EmailAlreadyExistsAsync", "Account", ErrorMessage = "User email already in use.")]
        public string Email { get; set; }

    }
}