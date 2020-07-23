using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete {
    // INFO : Authorization ve Authentication gibi işlemler için normalde JWT benzeri bir token yapısı için kullanılabilir
    public class User : EntityBase {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool Status { get; set; }

        public byte[] PasswordSalt { get; set; }

        public byte[] PasswordHash { get; set; }
    }
}