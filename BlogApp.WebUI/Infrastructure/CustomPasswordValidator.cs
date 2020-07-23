using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Core.Constants;

namespace BlogApp.WebUI.Infrastructure {
    public class CustomPasswordValidator : IPasswordValidator<AppUser> {

        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password) {

            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName.ToLower())) {
                errors.Add(new IdentityError {
                    Code = "SamePasswordAndUserName",
                    Description = ConstStrings.SamePasswordAndUserName
                });
            }

            return Task.FromResult(errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success);
        }
    }
}
