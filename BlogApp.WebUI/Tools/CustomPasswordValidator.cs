using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Core.Constants;
using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.WebUI.Infrastructure {
    public class CustomPasswordValidator : IPasswordValidator<AppUser> {

        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password) {

            var errors = new List<IdentityError>();

            if (password.Contains(user.UserName, System.StringComparison.CurrentCultureIgnoreCase)) {
                errors.Add(new IdentityError {
                    Code = "SamePasswordAndUserName",
                    Description = Strings.SamePasswordAndUserName
                });
            }

            return Task.FromResult(errors.Any()
                ? IdentityResult.Failed([.. errors])
                : IdentityResult.Success);
        }
    }
}
