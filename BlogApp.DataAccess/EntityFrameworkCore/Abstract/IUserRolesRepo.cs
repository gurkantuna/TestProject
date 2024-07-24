using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.DataAccess.EntitFrameworkCore.Abstract {
    public interface IUserRolesRepo {
        IEnumerable<IdentityUserRole<string>> GetUserRoles();
        IEnumerable<IdentityUserRole<string>> GetUserRoles(string userId);

        void DeleteUserRoles(string userId, string roleId);
    }
}
