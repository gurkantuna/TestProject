using System.Collections.Generic;
using System.Linq;
using BlogApp.DataAccess.Concrete.EntityFrameworkCore.Context;
using BlogApp.DataAccess.EntitFrameworkCore.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.DataAccess.EntitFrameworkCore.Concrete {
    public class UserClaimsRepo : IUserRolesRepo {

        public IEnumerable<IdentityUserRole<string>> GetUserRoles() {
            using var context = new BlogDbContext();
            return context.UserRoles.ToList();
        }

        public IEnumerable<IdentityUserRole<string>> GetUserRoles(string userId) {
            using var context = new BlogDbContext();
            return context.UserRoles
                .Where(x => x.UserId == userId)
                .ToList();
        }

        public void DeleteUserRoles(string userId, string roleId) {
            using var context = new BlogDbContext();
            var userRole = new IdentityUserRole<string> {
                UserId = userId,
                RoleId = roleId
            };

            context.UserRoles.Remove(userRole);
            context.SaveChanges();
        }
    }
}
