using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlogApp.DataAccess.Abstract {
    public interface IUserRolesDal {
        IEnumerable<IdentityUserRole<string>> GetUserRoles();
        IEnumerable<IdentityUserRole<string>> GetUserRoles(string userId);

        void DeleteUserRoles(string userId, string roleId);
    }
}
