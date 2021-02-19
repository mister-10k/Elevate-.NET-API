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
        public async Task<UserModel> CreateUserAsync(UserModel userModel)
        {
            UserModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var createdAt = DateTime.Now;
                    var user = new User
                    {
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        Email = userModel.Email,
                        Password = userModel.Password,
                        CompanyId = userModel.CompanyId,
                        UserTypeId = userModel.UserTypeId,
                        IsActive = true,
                        CreatedAt = createdAt,
                    };
                    dbContext.Users.Add(user);
                    if ((await dbContext.SaveChangesAsync()) > 0)
                    {
                        ret = userModel;
                        ret.CreatedAt = createdAt;
                        ret.CreatedAtText = createdAt.ToString("MM/dd/yyy");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("User Data Layer: CreateUserAsync Exception Msg", ex.Message);
            }

            return ret;
        }

        public async Task<UserModel> GetUserAsync(string email, string password)
        {
            UserModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                     ret = await (from U in dbContext.Users
                                  join C in dbContext.Companies on U.CompanyId equals C.ID
                                  where U.IsActive && C.IsActive && U.Email == email && U.Password == password
                                  select new UserModel {
                                      Id = U.ID,
                                      FirstName = U.FirstName,
                                      LastName = U.LastName,
                                      Email = U.Email,
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
                Console.WriteLine("User Data Layer: GetUserAsync Exception Msg", ex.Message);
            }

            return ret;
        }

        public async Task<SignUpMasterDataModel> GetSignUpMasterDataAsync()
        {
            SignUpMasterDataModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    ret = new SignUpMasterDataModel
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


        private async Task<List<ListItem>> GetComapniesForSignUpMasterDataAsync(ElevateEntities dbContext)
        {
            return await (from C in dbContext.Companies
                          where C.IsActive
                          select new
                          {
                              Value = C.ID,
                              Text = C.DisplayName
                          }).Select(x => new ListItem
                          {
                              Value = x.Value.ToString(),
                              Text = x.Text
                          }).ToListAsync();
        }

        private async Task<List<ListItem>> GetUserTypesForSignUpMasterDataAsync(ElevateEntities dbContext)
        {
            return await (from UT in dbContext.UserTypes
                          where UT.IsActive && UT.Name != AppConstants.UserType.Employee
                          select new
                          {
                              Value = UT.ID,
                              Text = UT.DisplayName
                          }).Select(x => new ListItem
                          {
                              Value = x.Value.ToString(),
                              Text = x.Text
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
