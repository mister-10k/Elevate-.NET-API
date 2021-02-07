using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Business
{
    public class UsersBL : IUsersBL
    {
        private readonly IUsersDL usersDL;
        public UsersBL(IUsersDL usersDL)
        {
            this.usersDL = usersDL;
        }

        public string Test()
        {
            return usersDL.Test();
        }
    }
}
