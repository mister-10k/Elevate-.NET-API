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

        public UserModel CreateUser(UserModel userModel)
        {
            return usersDL.CreateUser(userModel);
        }

        public UserModel GetUser(string email, string password)
        {
            return usersDL.GetUser(email, password);
        }

        public SignUpMasterDataModel GetSignUpMasterData()
        {
            return usersDL.GetSignUpMasterData();
        }

        public bool UserAlreadyHasEmail(string email)
        {
            return this.usersDL.UserAlreadyHasEmail(email);
        }
    }
}
