using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebUI.Models {
    public class UpdateUserModel {

        [Required]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
