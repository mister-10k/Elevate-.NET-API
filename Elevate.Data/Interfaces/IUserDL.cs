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
        /// <param name="userDTO">The user information</param>
        /// <returns>The user that has been created as a task</returns>
        Task<UserDTO> CreateUserAsync(UserDTO userDTO);

        /// <summary>Gets user details</summary>
        /// <param name="email">email of user</param>
        /// <returns>details of user as a task</returns>
        Task<UserDTO> GetUserByEmailAsync(string email);

        /// <summary>Gets sign up master data</summary>
        /// <returns>master data for sign up as a task</returns>
        Task<SignUpMasterDataDTO> GetSignUpMasterDataAsync();

        /// <summary>Check whether user already has given email</summary>
        ///  <param name="email">The email being checked</param>
        /// <returns>tru of user already has email and false if otherwise as a task</returns>
        Task<bool> UserAlreadyHasEmailAsync(string email);
    }
}
