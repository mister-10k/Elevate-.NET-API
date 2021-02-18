using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Shared
{
    public interface IUserDL
    {
        /// <summary>Creates new user</summary>
        /// <param name="userModel">The user information</param>
        /// <returns>The user that has been created</returns>
        UserModel CreateUser(UserModel userModel);

        /// <summary>Gets user details</summary>
        /// <param name="email">email of user</param>
        /// <param name="password">password of user</param>
        /// <returns>details of user</returns>
        UserModel GetUser(string email, string password);

        /// <summary>Gets sign up master data</summary>
        /// <returns>master data for sign up</returns>
        SignUpMasterDataModel GetSignUpMasterData();

        /// <summary>Check whether user already has given email</summary>
        ///  <param name="email">The email being checked</param>
        /// <returns>tru of user already has email and false if otherwise</returns>
        bool UserAlreadyHasEmail(string email);
    }
}
