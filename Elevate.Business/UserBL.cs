using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Business
{
    public class UserBL : IUserBL
    {
        private readonly IUserDL usersDL;
        public UserBL(IUserDL usersDL)
        {
            this.usersDL = usersDL;
        }

        public async Task<UserModel> CreateUserAsync(UserModel userModel)
        {
            return await usersDL.CreateUserAsync(userModel);
        }

        public async Task<UserModel> GetUserAsync(string email, string password)
        {
            return await usersDL.GetUserAsync(email, password);
        }

        public async Task<SignUpMasterDataModel> GetSignUpMasterDataAsync()
        {
            return await usersDL.GetSignUpMasterDataAsync();
        }

        public async Task<bool> UserAlreadyHasEmailAsync(string email)
        {
            return await this.usersDL.UserAlreadyHasEmailAsync(email);
        }
    }
}
