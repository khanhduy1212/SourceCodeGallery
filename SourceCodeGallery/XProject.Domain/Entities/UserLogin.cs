using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using NS.Entity;

namespace XProject.Domain.Entities
{
    public class UserLogin : EntityBase
    {
        public UserLogin()
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            Roles = new HashSet<Role>();
            Status = EntityStatus.Normal;

        }

        #region Primitive

        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Picture { get; set; }
        public DateTime? LastAccess { get; set; }
        public string Address { get; set; }
        public string AddressEnglish { get; set; }
        public string FaxNo { get; set; }
        public string Website { get; set; }
        public EntityStatus Status { get; set; }
        #endregion

        #region Navigation properties

        public virtual ICollection<Role> Roles { get; set; }

        // LINQ expression

        public int? LanguageID { get; set; }
        [ForeignKey("LanguageID")]
        public virtual Language Language { get; set; }
        #endregion

        #region Helpers

        public static Func<UserLogin, bool> HasName(string username)
        {
            return u => u.DisplayName.Trim().ToLower() == username.Trim().ToLower();
        }

        #endregion
    }
}
