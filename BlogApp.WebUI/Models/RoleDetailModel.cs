using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlogApp.WebUI.Models {
    public class RoleDetailModel {
        public IdentityRole Role { get; set; }

        public IEnumerable<AppUser> AlreadyMembers { get; set; }
        public IEnumerable<AppUser> ToMembers { get; set; }
    }
}
