using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Shared
{
    public interface IUserBL
    {

        /// <summary>Creates new user</summary>
        /// <param name="userModel">The user information</param>
        /// <returns>The user that has been created as a task</returns>
        Task<UserModel> CreateUserAsync(UserModel userModel);

        /// <summary>Gets user details</summary>
        /// <param name="email">email of user</param>
        /// <param name="password">password of user</param>
        /// <returns>details of user as a task</returns>
        Task<UserModel> GetUserAsync(string email, string password);

        /// <summary>Gets sign up master data</summary>
        /// <returns>master data for sign up as a task</returns>
        Task<SignUpMasterDataModel> GetSignUpMasterDataAsync();

        /// <summary>Check whether user already has given email</summary>
        ///  <param name="email">The email being checked</param>
        /// <returns>true of user already has email and false if otherwise as a task</returns>
        Task<bool> UserAlreadyHasEmailAsync(string email);
    }
}
