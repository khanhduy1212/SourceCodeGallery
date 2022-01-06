using XProject.Domain.Concrete;
using XProject.Domain.Entities;

namespace XProject.Domain.Abstract
{
    public interface ILoginTracker
    {
        void SignIn(string email, string hashKey);
        void SignOut(string username, string hashKey = null);
        bool IsLogged(string username);

        /// <summary>
        /// Retrieve UserLogin from memory
        /// </summary>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        UserLogin RetrieveUser(string hashKey);

        /// <summary>
        /// Update new info for all records associated with old username
        /// </summary>
        /// <param name="oldEmail"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        UserLogin ReloadUser(string oldEmail, UserLogin info);

        #region Customer Login

        bool IsCustomerLogged(string username);
        void CustomerSignIn(string email, string hashKey);
        void CustomerSignOut(string username, string hashKey = null);
        //BDSAccount RetrieveCustomer(string hashKey);
        //BDSAccount ReloadCustomer( BDSAccount info);
        #endregion Customer Login
       
        //InMemLoginRecord RetrieveCustomerInfo(BDSAccount contact);
        void CustomerSignInExternal(string typeExternal, string sessionSessionId);
    }
}
