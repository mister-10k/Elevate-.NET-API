using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Elevate.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUsersBL usersBL;
        public UsersController(IUsersBL usersBL)
        {
            this.usersBL = usersBL;
        }

        public string Get(int id)
        {
            return usersBL.Test();
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
