using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using XProject.Domain;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Domain.Enum;

namespace XProject.Domain.Concrete
{
    public class EFRoleBasedAuthorizer : Repository, IRoleService
    {
        public EFRoleBasedAuthorizer(DbContext db)
            : base(db)
        {
        }

        #region IRoleService Members

        public IEnumerable<Role> GetAllRoles()
        {
            return GetAll<Role>(r => r.Permissions).AsEnumerable();
        }

        public IEnumerable<Role> GetAllRoles(Func<Role, bool> predicate)
        {
            return GetAll<Role>(predicate,r => r.Permissions).AsEnumerable();
        }

        public IEnumerable<Role> GetUserRoles(int userID)
        {
            var user = Get<UserLogin>(u => u.ID == userID,
                                 u => u.Roles);

            if (user == null)
                return new Role[] { };

            return user.Roles;
        }

        public IEnumerable<Role> GetUserRoles(string email)
        {
            var user = Get<UserLogin>(u => u.Email == email,
                                 u => u.Roles);

            if (user == null)
                return new Role[] { };

            return user.Roles;
        }

        public Role GetRole(int id)
        {
            Role role = Get<Role>(r => r.ID == id,
                r => r.Permissions);
            return role;
        }

        public Role GetRoleByName(string name)
        {
            return Get(Role.HasName(name));
        }

        public IEnumerable<Role> GetAllRoleByLevel(RoleLevel level)
        {
            return GetAll<Role>(m => m.RoleLevel == level);
        }

        public Role CreateRole(Role role, IEnumerable<int> rolePermissions)
        {
            role.Permissions = GetAll<Permission>(p => rolePermissions.Contains(p.ID)).ToList();
            return Create(role);
        }

        public Role UpdateRole(int id, Role roleInfo, IEnumerable<int> rolePermissions)
        {
            var role = Get<Role>(id);

            role.Name = roleInfo.Name;
            role.RoleLevel = roleInfo.RoleLevel;

            role.Permissions.Clear();
            role.Permissions = GetAll<Permission>(p => rolePermissions.Contains(p.ID)).ToList();

            Update(role);

            return role;
        }

        public void DeleteRole(int id)
        {
            Delete<Role>(id);
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return GetAll<Permission>();
        }

        public virtual Permission CreatePermission(Permission perm)
        {
            return Create(perm);
        }

        public IEnumerable<Permission> GetUserPermissions(int userID)
        {
            var user = Get<UserLogin>(userID);

            if (user == null)
                return new List<Permission>();

            return user.Roles.SelectMany(r => r.Permissions).AsEnumerable();
        }

        public Permission EnsurePermissionRecord(string target, string right, string description = null)
        {
            return Get<Permission>(p => p.Target.ToLower() == target.ToLower() &&
                                        p.Right.ToLower() == right.ToLower() &&
                                        (string.IsNullOrEmpty(description) || p.Description == description))
                   ?? CreatePermission(new Permission { Target = target, Right = right, Description = description });
        }

        public void AssignRoles(UserLogin userLogin, IEnumerable<int> userRoles)
        {
            var userExist = Get<UserLogin>(userLogin.ID);
            //if (UserLogin == null) return;
            userExist.Roles.Clear();
            userExist.Roles = GetAll<Role>(r => userRoles.Contains(r.ID)).ToList();
            userLogin.Roles.Clear();
            userLogin.Roles = GetAll<Role>(r => userRoles.Contains(r.ID)).ToList();
            var tmp = Update(userExist);
            //_db.SaveChanges();
        }

        //public void AssignBranches(UserLogin UserLogin, IEnumerable<int> branches)
        //{
        //    var userExist = Get<UserLogin>(UserLogin.ID);
        //    if (userExist == null) return;
        //    userExist.Branches.Clear();
        //    userExist.Branches = GetAll<Make>(r => branches.Contains(r.ID)).ToList();
        //    UserLogin.Branches.Clear();
        //    UserLogin.Branches = GetAll<Branch>(r => branches.Contains(r.ID)).ToList();
        //    Update(userExist);
        //    //_db.SaveChanges();
        //}

        public bool CheckAccess(int userID, string feature, string permissionType = null)
        {
            string[] segments = feature.Split(new[] { '.' });
            if (segments.Length < 2) return false;

            string controller = segments[0];
            string action = segments[1];
            return CheckAccess(userID, controller, action, permissionType);
        }

        #endregion

        public virtual bool CheckAccess(int userID, string controller, string action, string permissionType = null)
        {
            var user = Get<UserLogin>(u => u.ID == userID,
                                 u => u.Roles);
            if (user == null) return false;

            // Admin
            if (user.Roles.Select(r => r.ID).Contains(1))
                return true;

            IEnumerable<Permission> userPermissions = GetUserPermissions(user.ID);
            if (!string.IsNullOrEmpty(permissionType))
                return userPermissions.Any(Permission.HasRight(controller, action, permissionType));
            return userPermissions.Any(Permission.HasRight(controller, action));
        }


        public void AssignBranches(UserLogin userLogin, IEnumerable<int> branches)
        {
            throw new NotImplementedException();
        }
    }
}
