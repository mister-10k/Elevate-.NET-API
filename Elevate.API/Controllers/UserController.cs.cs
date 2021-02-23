using Elevate.Shared;
using Elevate.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Elevate.Business;

namespace Elevate.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserBL userBL;
        private readonly IMapper mapper;
        public UserController(IUserBL userBL, IMapper mapper)
        {
            this.userBL = userBL;
            this.mapper = mapper;
        }

        [Route("api/user/login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Login(UserModel userModel)
        {
            if (userModel != null)
            {
                var result = await userBL.LoginAsync(userModel.Email, userModel.Password);
                if (result != null)
                {
                    return Ok(TokenManager.GenerateToken(result.Email, result.CompanyId));
                }
            }
 
            return Unauthorized();
        }

        [Route("api/user/GetSignUpMasterData")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> GetSignUpMasterData()
        {
            var result = await userBL.GetSignUpMasterDataAsync();
            if (result != null)
            {
                return Ok(mapper.Map<SignUpMasterDataModel>(result));
            }

            return Content(HttpStatusCode.NotFound, AppConstants.HttpErrorMessage.ResourceNotFound);
        }

        [Route("api/user/UserAlreadyHasEmail")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> UserAlreadyHasEmail(UserModel user)
        {
            return Ok(await userBL.UserAlreadyHasEmailAsync(user.Email));
        }

        [Route("api/user/SignUp")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> SignUp(UserModel userModel)
        {   
            if (userModel != null)
            {
                var userDTO = mapper.Map<UserDTO>(userModel);

                var result = await userBL.CreateUserAsync(userDTO);
                if (result != null)
                {
                    return Ok(TokenManager.GenerateToken(result.Email, result.CompanyId));
                }
            }


            return Unauthorized();
        }
    }
}
