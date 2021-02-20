using Elevate.Entities;
using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Data
{
    public class UserDL : IUserDL
    {
        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            UserDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var createdAt = DateTime.Now;
                    var user = new User
                    {
                        FirstName = userDTO.FirstName,
                        LastName = userDTO.LastName,
                        Email = userDTO.Email,
                        Password = userDTO.Password,
                        CompanyId = userDTO.CompanyId,
                        UserTypeId = userDTO.UserTypeId,
                        IsActive = true,
                        CreatedAt = createdAt,
                    };
                    dbContext.Users.Add(user);
                    if ((await dbContext.SaveChangesAsync()) > 0)
                    {
                        ret = userDTO;
                        ret.CreatedAt = createdAt;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("User Data Layer: CreateUserAsync Exception Msg", ex.Message);
            }

            return ret;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            UserDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                     ret = await (from U in dbContext.Users
                                  join C in dbContext.Companies on U.CompanyId equals C.ID
                                  where U.IsActive && C.IsActive && U.Email == email
                                  select new UserDTO {
                                      Id = U.ID,
                                      FirstName = U.FirstName,
                                      LastName = U.LastName,
                                      Email = U.Email,
                                      Password = U.Password,
                                      CompanyId = C.ID,
                                      CompanyName = C.Name,
                                      CompanyDisplayName = C.DisplayName,
                                      CreatedAt = U.CreatedAt,
                                      ModifiedAt = U.ModifiedAt
                                  }).FirstOrDefaultAsync();
                }      
            }
            catch (Exception ex)
            {
                Console.WriteLine("User Data Layer: LoginAsync Exception Msg", ex.Message);
            }

            return ret;
        }

        public async Task<SignUpMasterDataDTO> GetSignUpMasterDataAsync()
        {
            SignUpMasterDataDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    ret = new SignUpMasterDataDTO
                    {
                        Companies = await GetComapniesForSignUpMasterDataAsync(dbContext),
                        UserTypes = await GetUserTypesForSignUpMasterDataAsync(dbContext)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: GetSignUpMasterDataAsync Exception Msg", ex.Message);
            }

            return ret;
        }


        private async Task<List<ComapnyDTO>> GetComapniesForSignUpMasterDataAsync(ElevateEntities dbContext)
        {
            return await (from C in dbContext.Companies
                          where C.IsActive
                          select new ComapnyDTO
                          {
                              Id = C.ID,
                              Name = C.Name,
                              DisplayName = C.DisplayName,
                              CreatedAt = C.CreatedAt,
                              ModifiedAt = C.ModifiedAt,
                              IsActive = C.IsActive
                          }).ToListAsync();
        }

        private async Task<List<UserTypeDTO>> GetUserTypesForSignUpMasterDataAsync(ElevateEntities dbContext)
        {
            return await (from UT in dbContext.UserTypes
                          where UT.IsActive && UT.Name != AppConstants.UserType.Employee
                          select new UserTypeDTO
                          {
                              Id = UT.ID,
                              Name = UT.Name,
                              DisplayName = UT.DisplayName,
                              CreatedAt = UT.CreatedAt,
                              ModifiedAt = UT.ModifiedAt,
                              IsActive = UT.IsActive
                          }).ToListAsync();
        }

        public async Task<bool> UserAlreadyHasEmailAsync(string email)
        {
            bool ret = false;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    ret = (await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email)) != null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: UserAlreadyHasEmailAsync Exception Msg", ex.Message);
            }

            return ret;
        }
    }
}
