using System.Collections.Generic;
using XProject.Domain.Entities;
using XProject.Domain.Enum;

namespace XProject.Domain.Abstract
{
    public interface IRoleService : IAuthorizationService
    {
        IEnumerable<Role> GetAllRoles();
        IEnumerable<Role> GetUserRoles(int userID);
        IEnumerable<Role> GetUserRoles(string username);
        Role GetRole(int id);
        Role GetRoleByName(string name);
        IEnumerable<Role> GetAllRoleByLevel(RoleLevel level);
        Role CreateRole(Role roleInfo, IEnumerable<int> rolePermissions);
        Role UpdateRole(int id, Role roleInfo, IEnumerable<int> rolePermissions);
        void DeleteRole(int id);

        IEnumerable<Permission> GetPermissions();
        Permission CreatePermission(Permission perm);
        IEnumerable<Permission> GetUserPermissions(int userID);
        Permission EnsurePermissionRecord(string target, string right, string description = null);

        void AssignRoles(UserLogin userLogin, IEnumerable<int> userRoles);
        void AssignBranches(UserLogin userLogin, IEnumerable<int> branches);
    }
}
