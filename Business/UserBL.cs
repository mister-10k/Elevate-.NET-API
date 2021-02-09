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

        public string Test()
        {
            return usersDL.Test();
        }
    }
}
