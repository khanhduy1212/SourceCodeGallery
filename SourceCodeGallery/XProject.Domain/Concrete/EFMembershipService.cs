using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NS.Entity;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Domain.Enum;
using XProject.Domain.Helpers;

namespace XProject.Domain.Concrete
{
    public class EFMembershipService : Repository, IMembershipService
    {
        public EFMembershipService(DbContext db)
            : base(db)
        {
        }

        #region IMembershipService Members

        public IEnumerable<UserLogin> GetUsers(Func<UserLogin, bool> predicate)
        {
            return GetAll(predicate, u => new
            {
                Roles = u.Roles.Select(r => r.Permissions),
                u.Language
            }).OrderBy(u => u.DisplayName)
                .AsEnumerable();
        }
        public IEnumerable<UserLogin> GetUsers()
        {
            return GetAll<UserLogin>(u => new
                                     {
                                         Roles = u.Roles.Select(r => r.Permissions),
                                         u.Language
                                     })
                .OrderBy(u => u.DisplayName)
                .AsEnumerable();
        }
        public IEnumerable<UserLogin> GetUsersDeleted()
        {
            return GetAll<UserLogin>(u => new
            {
                Roles = u.Roles.Select(r => r.Permissions),
            })
                .OrderBy(u => u.DisplayName)
                .AsEnumerable();
        }


        public bool UpdateUser(UserLogin userLoginInfo)
        {
            return Update(userLoginInfo);
        }

        public void UpdateUserLanguage(int id, int languageID)
        {
            UserLogin userLogin = GetUser(id);
            if (userLogin == null) return;
            if (userLogin.LanguageID != languageID)
            {
                userLogin.LanguageID = languageID;
                userLogin.Language = null;
                Update(userLogin);
                //_db.SaveChanges();
            }
        }

        public void UpdateUserPicture(int id, string fileName)
        {
            UserLogin userLogin = GetUser(id);
            if (userLogin == null) return;

            userLogin.Picture = fileName;
            Update(userLogin);
            //_db.SaveChanges();
        }

        public virtual UserLogin GetUser(int id)
        {
            return Get<UserLogin>(u => u.ID == id,
                             u => u.Roles.Select(r => r.Permissions),
                             u => u.Language);
        }
        public UserLogin GetUserByName(string username)
        {
            return string.IsNullOrWhiteSpace(username)
                       ? null
                       : Get<UserLogin>(u => u.DisplayName != null &&
                                        u.DisplayName.ToLower() == username.ToLower(),
                                   u => u.Roles.Select(r => r.Permissions),
                                   u => u.Language);
        }
        public UserLogin GetUserByEmail(string email)
        {
            return string.IsNullOrWhiteSpace(email)
                       ? null
                       : Get<UserLogin>(u => u.Email != null &&
                                        u.Email.Trim().ToLower() == email.Trim().ToLower(),
                                   u => u.Roles.Select(r => r.Permissions),
                                   u => u.Language);
        }
        public IEnumerable<UserLogin> GetAllUserByEmail(string email)
        {
            return GetAll<UserLogin>(m => m.Email.Trim().ToLower() == email.Trim().ToLower()).ToList();
        }
        public UserLogin ValidateLogin(string email, string password)
        {
            password = EncryptHelper.EncryptPassword(password);
            return Get<UserLogin>(u => u.Email != null &&
                                  u.Email.ToLower() == email.ToLower() &&
                                  u.Password == password && u.Status == EntityStatus.Normal);
        }

        public UserLogin CreateUser(UserLogin userLogin)
        {
            userLogin.Email = userLogin.Email.Trim();
            userLogin.Password = EncryptHelper.EncryptPassword(userLogin.Password);
            return Create(userLogin);
        }

        public bool DeleteUser(int id)
        {
            var user = GetUser(id);
            user.Status = EntityStatus.Deleted;
            return Update(user);
        }
        public IEnumerable<UserLogin> GetAllUser()
        {
            return GetAll<UserLogin>();
        }

        public IEnumerable<UserLogin> GetAllUser(Func<UserLogin, bool> predicate)
        {
            return GetAll<UserLogin>(predicate);
        }

        public bool ReActiveUser(int id)
        {
            var user = GetUser(id);
            user.Status = EntityStatus.Normal;
            return Update(user);
        }
        public IEnumerable<UserLogin> GetUsersInRole(params string[] roleName)
        {
            return GetAll<UserLogin>(u => u.Roles
                                      .Select(r => r.Name.ToUpper())
                                      .Intersect(roleName.Select(r1 => r1.ToUpper()))
                                      .Any(),
                                u => u.Roles.Select(r => r.Permissions)).AsEnumerable();
        }

        public IEnumerable<UserLogin> GetUsersContainsInRole(params string[] roleName)
        {
            return GetAll<UserLogin>(u => u.Roles
                                      .Select(r => r.Name.ToUpper())
                                      .Intersect(roleName.Select(r1 => r1.ToUpper()))
                                      .Any(),
                                u => u.Roles.Select(r => r.Permissions)).AsEnumerable();
        }



       
        #endregion


        
    }
}
