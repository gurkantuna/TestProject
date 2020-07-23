using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebUI.Models {
    public class RegisterUserModel {

        [Required]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
