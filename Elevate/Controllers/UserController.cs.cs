using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Elevate.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserBL userBL;
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        [Route("api/user/login")]
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage Login(UserModel userModel)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var user = userBL.GetUser(userModel.Email, userModel.Password);
            if (user != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(user.Email, user.CompanyId));
            }

            return response;
        }

        [Route("api/user/GetSignUpMasterData")]
        [AllowAnonymous]
        [HttpGet]
        public SignUpMasterDataModel GetSignUpMasterData()
        {
            return userBL.GetSignUpMasterData();
        }

        [Route("api/user/UserAlreadyHasEmail")]
        [HttpPost]
        public bool UserAlreadyHasEmail(UserModel user)
        {
            return userBL.UserAlreadyHasEmail(user.Email);
        }

        [Route("api/user/SignUp")]
        [HttpPost]
        public string SignUp(UserModel userModel)
        {
            string token = null;
            var user = userBL.CreateUser(userModel);
            if (user != null)
            {
                token = TokenManager.GenerateToken(user.Email, user.CompanyId);
            }

            return token;
        }
    }
}
