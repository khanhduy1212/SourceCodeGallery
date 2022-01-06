using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Domain.Helpers;

namespace XProject.Domain.Concrete
{
    public class InMemLoginTracker : EFMembershipService, ILoginTracker
    {
        public static readonly List<InMemLoginRecord> LoginRecords = new List<InMemLoginRecord>();
        public static readonly List<InMemLoginRecord> CustomerLoginRecords = new List<InMemLoginRecord>();
        public InMemLoginTracker(DbContext db)
            : base(db)
        {
        }

        #region ILoginTracker Members

        public virtual void SignIn(string email, string hashKey)
        {
            if (email == null)
                throw new ArgumentNullException("email");

            if (CanSignIn(email))
                SignIn(GetUserByEmail(email), hashKey);
        }

        public UserLogin RetrieveUser(string hashKey)
        {
            InMemLoginRecord record = LoginRecords.FirstOrDefault(r => r.SessionID == hashKey && r.UserLogin != null);

            return record != null ? record.UserLogin : null;
        }

        public UserLogin ReloadUser(string oldEmail, UserLogin info)
        {
            if (string.IsNullOrEmpty(oldEmail))
                throw new ArgumentNullException(oldEmail);

            foreach (InMemLoginRecord record in LoginRecords)
            {
                if (record.UserLogin != null && record.UserLogin.Email == oldEmail)
                {
                    record.UserLogin = info;
                }
            }
            return info;
        }

        /// <summary>
        ///     Sign out a specific UserLogin
        /// </summary>
        /// <param name="username">Is username or email</param>
        /// <param name="hashKey">Session ID</param>
        public void SignOut(string username, string hashKey = null)
        {
             if (!string.IsNullOrEmpty(hashKey) && string.IsNullOrEmpty(username))
            {
                LoginRecords.RemoveAll(r => r.UserLogin != null &&
                                            r.SessionID == hashKey);
            }
            else if (!string.IsNullOrEmpty(hashKey))
            {
                LoginRecords.RemoveAll(r => r.UserLogin != null &&
                                            (r.UserLogin.DisplayName.ToLower() == username.ToLower() ||
                                             r.UserLogin.Email.ToLower() == username.ToLower()) &&
                                            r.SessionID == hashKey);
            }
            else
            {
                LoginRecords.RemoveAll(r => r.UserLogin != null &&
                                            (r.UserLogin.DisplayName.ToLower() == username.ToLower() ||
                                             r.UserLogin.Email.ToLower() == username.ToLower()));
            }
        }

        public bool IsLogged(string username)
        {
            return LoginRecords.Any(r => r.UserLogin.DisplayName.ToLower() == username.ToLower() ||
                                         r.UserLogin.Email.ToLower() == username.ToLower());
        }


        protected void SignIn(UserLogin userLogin, string hashKey)
        {
            InMemLoginRecord existingRecord = LoginRecords.FirstOrDefault(r => r.SessionID == hashKey);
            if (existingRecord != null)
            {
                existingRecord.UserLogin = userLogin;
            }
            else
                LoginRecords.Add(new InMemLoginRecord(userLogin, hashKey));
        }

        protected virtual bool CanSignIn(string username)
        {
            return true;
        }

        public override UserLogin GetUser(int id)
        {
            InMemLoginRecord loginRecord =
                LoginRecords.FirstOrDefault(
                    r => r.UserLogin != null &&
                        r.UserLogin.ID == id);
            if (loginRecord != null)
                return loginRecord.UserLogin;
            return base.GetUser(id);
        }

        #endregion


      
        protected virtual bool CustomerCanSignIn(string username)
        {
            return true;
        }
        protected virtual bool CustomerCanSignInExternal(string idExternal)
        {
            return true;
        }


        public virtual void CustomerSignIn(string email, string hashKey)
        {
            if (email == null)
                throw new ArgumentNullException("email");

            if (CustomerCanSignIn(email))
            {
                //var contactRepo = new EFGeneralRepository<Contact>(_db);
                //var contact = contactRepo.GetAllTEntities().FirstOrDefault(m => m.Email == email);
                //CustomerSignIn(contact, hashKey);

                //var contactRepo = new EFGeneralRepository<BDSAccount>(_db);
              
                //var user = contactRepo.GetIQueryableItems().FirstOrDefault(m => m.Email == email);
                //if (user != null)
                //{
                 
                //    CustomerSignIn(user, hashKey);
                //}
            }
        }

        public void CustomerSignOut(string username, string hashKey = null)
        {

            if (username == null)
                throw new ArgumentNullException("username");
          
            //if (!string.IsNullOrEmpty(hashKey) && string.IsNullOrEmpty(username))
            //{
            //    CustomerLoginRecords.RemoveAll(r => r.Customer != null && r.SessionID == hashKey);
            //}
            //else if (!string.IsNullOrEmpty(hashKey))
            //{
          
            //    CustomerLoginRecords.RemoveAll(r => r.Customer != null &&
            //                                ((!string.IsNullOrWhiteSpace(r.Customer.Email) && r.Customer.Email.ToLower() == username.ToLower())|| r.Customer.ExternalId==username.Trim()) &&
            //                                r.SessionID == hashKey);
            //}
            //else
            //{
            //    CustomerLoginRecords.RemoveAll(r => r.Customer != null &&
            //                                        ((string.IsNullOrWhiteSpace(r.Customer.Email) && r.Customer.Email.ToLower() == username.ToLower()) || r.Customer.ExternalId == username.Trim()));
            //}
            HttpContext.Current.Session["AccountId"] = null;
            HttpContext.Current.Session["AccountEmail"] = null;
            HttpContext.Current.Session["PersonalId"] = null;
            HttpContext.Current.Session["EmployerId"] = null;
            HttpContext.Current.Session.Remove("AccountId");
            HttpContext.Current.Session.Remove("AccountEmail");
            HttpContext.Current.Session.Remove("PersonalId");
            HttpContext.Current.Session.Remove("EmployerId");
        }
        public  void AddSession(string emailAccount, int id)
        {

            HttpContext.Current.Session["AccountEmail"] = emailAccount;
            HttpContext.Current.Session["AccountId"] = id;
          //  var type = new EFAccount(_db).GetUser(id, false, false);

            //if (type.TypeRegister == 1)
            //{
            //    var idEmployer = new EFGeneralRepository<BDSEmployer>(_db).GetIQueryableItems().FirstOrDefault(x => x.IdAccount == id);
            //    if (idEmployer != null)
            //    {
                   
            //        HttpContext.Current.Session["EmployerId"] = idEmployer.ID;
            //    }
            //}
            //if (type.TypeRegister == 2)
            //{
            //    var idPersonal = new EFGeneralRepository<BDSPersonal>(_db).GetIQueryableItems().FirstOrDefault(x => x.IdAccount == id);
            //    if (idPersonal != null)
            //    {
             
            //        HttpContext.Current.Session["PersonalId"] = idPersonal.ID;
            //    }
            //}

        }
        //public BDSAccount RetrieveCustomer(string hashKey)
        //{

        //    InMemLoginRecord record = CustomerLoginRecords.FirstOrDefault(r => r.SessionID == hashKey && r.Customer != null);
        //    if (record != null)
        //    {
        //        AddSession(record.Customer.Email, record.Customer.ID);

        //    }

        //    return record != null ? record.Customer : null;
        //}

        //public BDSAccount ReloadCustomer( BDSAccount info)
        //{


        //    for (int i = 0; i < CustomerLoginRecords.Count; i++)
        //    {

        //        if (CustomerLoginRecords[i].Customer != null && CustomerLoginRecords[i].Customer.ID == info.ID)
        //        {
        //            CustomerLoginRecords[i].Customer = info;
        //            if (info.TypeRegister == 1)
        //            {
        //                var idEmployer = new EFGeneralRepository<BDSEmployer>(_db).GetIQueryableItems().FirstOrDefault(x => x.IdAccount == info.ID);
        //                if (idEmployer != null)
        //                {
        //                    CustomerLoginRecords[i].TypeCustomer = 1;
        //                    CustomerLoginRecords[i].Employer = idEmployer;
        //                }
        //            }
        //            if (info.TypeRegister == 2)
        //            {
        //                var idPersonal = new EFGeneralRepository<BDSPersonal>(_db).GetIQueryableItems().FirstOrDefault(x => x.IdAccount == info.ID);
        //                if (idPersonal != null)
        //                {
        //                    CustomerLoginRecords[i].TypeCustomer = 2;
        //                    CustomerLoginRecords[i].Personal = idPersonal;
        //                }
        //            }
        //        }
        //    }
        //    return info;
        //}




        public bool IsCustomerLogged(string username)
        {

            return true;// CustomerLoginRecords.Any(r => r.Customer.Email.ToLower() == username.ToLower());
        }
        //protected void CustomerSignIn(BDSAccount contact, string hashKey)
        //{

        //    InMemLoginRecord existingRecord = CustomerLoginRecords.FirstOrDefault(r => r.SessionID == hashKey);
        //    if (existingRecord != null)
        //    {
        //        existingRecord.Customer = contact;
        //    }
        //    else
        //    {
        //        InMemLoginRecord iMem = new InMemLoginRecord(contact, hashKey);

        //        CustomerLoginRecords.Add(iMem);
        //    }


        //    AddSession(contact.Email, contact.ID);
        //    ReloadCustomer(contact);
        //}


        //public InMemLoginRecord RetrieveCustomerInfo(BDSAccount contact)
        //{
        //    return CustomerLoginRecords.FirstOrDefault(t => t.Customer != null && t.Customer.ID == contact.ID);


        //}

        public void CustomerSignInExternal(string typeExternal, string sessionSessionId)
        {
            //if (typeExternal == null)
            //    throw new ArgumentNullException("External");

            //if (CustomerCanSignInExternal(typeExternal))
            //{
                

            //    var contactRepo = new EFGeneralRepository<BDSAccount>(_db);

            //    var user = contactRepo.GetIQueryableItems().FirstOrDefault(m => m.ExternalId == typeExternal);
            //    if (user != null)
            //    {

            //        CustomerSignIn(user, sessionSessionId);
            //    }
            //}
        }
    }

    public class InMemLoginRecord
    {
        public InMemLoginRecord(UserLogin userLogin, string sessionId)
        {
            UserLogin = userLogin;
            SessionID = sessionId;
        }
      
        public UserLogin UserLogin { get; set; }
     
        public string SessionID { get; set; }

        //public InMemLoginRecord(BDSAccount customer, string sessionId)
        //{
        //    Customer = customer;
        //    SessionID = sessionId;
        //}
        public int TypeCustomer { get; set; }
        //public BDSAccount Customer { get; set; }
        //public BDSEmployer Employer { get; set; }
        //public BDSPersonal Personal { get; set; }
    }
}
