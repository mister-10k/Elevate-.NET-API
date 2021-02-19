using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<HttpResponseMessage> Login(UserModel userModel)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var user = await userBL.GetUserAsync(userModel.Email, userModel.Password);
            if (user != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(user.Email, user.CompanyId));
            }

            return response;
        }

        [Route("api/user/GetSignUpMasterData")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<SignUpMasterDataModel> GetSignUpMasterData()
        {
            return await userBL.GetSignUpMasterDataAsync();
        }

        [Route("api/user/UserAlreadyHasEmail")]
        [HttpPost]
        public async Task<bool> UserAlreadyHasEmail(UserModel user)
        {
            return await userBL.UserAlreadyHasEmailAsync(user.Email);
        }

        [Route("api/user/SignUp")]
        [HttpPost]
        public async Task<string> SignUp(UserModel userModel)
        {
            string token = null;
            var user = await userBL.CreateUserAsync(userModel);
            if (user != null)
            {
                token = TokenManager.GenerateToken(user.Email, user.CompanyId);
            }

            return token;
        }
    }
}
