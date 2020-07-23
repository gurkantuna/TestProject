using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Infrastructure {

    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUsersTagHelper : TagHelper {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleUsersTagHelper(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HtmlAttributeName("identity-role")]
        public string RoleId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {

            List<string> users = new List<string>();

            var role = await _roleManager.FindByIdAsync(RoleId);

            if (role != null) {
                foreach (var user in _userManager.Users) {
                    if (user != null && await _userManager.IsInRoleAsync(user, role.Name)) {
                        users.Add(user.UserName);
                    }
                }
            }

            output.Content.SetContent(users.Count != 0 ? string.Join(",", users) : "No users");
        }
    }
}
