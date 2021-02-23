using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Elevate.Business
{
    public class UserBL : IUserBL
    {
        private readonly IUserDL usersDL;
        public UserBL(IUserDL usersDL)
        {
            this.usersDL = usersDL;
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            return await usersDL.CreateUserAsync(userDTO);
        }

        public async Task<UserDTO> LoginAsync(string email, string password)
        {
            var userDTO = await usersDL.GetUserByEmailAsync(email);
            if (userDTO != null)
            {
                var match = BC.Verify(password, userDTO.Password);
                if (match)
                {
                    return userDTO;
                }           
            }
                   
            return null;
        }

        public async Task<SignUpMasterDataDTO> GetSignUpMasterDataAsync()
        {
            return await usersDL.GetSignUpMasterDataAsync();
        }

        public async Task<bool> UserAlreadyHasEmailAsync(string email)
        {
            return await this.usersDL.UserAlreadyHasEmailAsync(email);
        }
    }
}
