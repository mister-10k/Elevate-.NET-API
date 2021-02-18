using Elevate.Entities;
using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Data
{
    public class UserDL : IUserDL
    {
        public UserModel CreateUser(UserModel userModel)
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
                    if (dbContext.SaveChanges() > 0)
                    {
                        ret = userModel;
                        ret.CreatedAt = createdAt;
                        ret.CreatedAtText = createdAt.ToString("MM/dd/yyy");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("User Data Layer: CreateUser Exception Msg", ex.Message);
            }

            return ret;
        }

        public UserModel GetUser(string email, string password)
        {
            UserModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                     ret = (from U in dbContext.Users
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
                            }).FirstOrDefault();
                }      
            }
            catch (Exception ex)
            {
                Console.WriteLine("User Data Layer: GetUser Exception Msg", ex.Message);
            }

            return ret;
        }

        public SignUpMasterDataModel GetSignUpMasterData()
        {
            SignUpMasterDataModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    ret = new SignUpMasterDataModel
                    {
                        Companies = GetComapniesForSignUpMasterData(dbContext),
                        UserTypes = GetUserTypesForSignUpMasterData(dbContext)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: GetEmployeeFormMasterData Exception Msg", ex.Message);
            }

            return ret;
        }


        private List<ListItem> GetComapniesForSignUpMasterData(ElevateEntities dbContext)
        {
            return (from C in dbContext.Companies
                    where C.IsActive
                    select new
                    {
                        Value = C.ID,
                        Text = C.DisplayName
                    }).Select(x => new ListItem
                    {
                        Value = x.Value.ToString(),
                        Text = x.Text
                    }).ToList();
        }

        private List<ListItem> GetUserTypesForSignUpMasterData(ElevateEntities dbContext)
        {
            return (from UT in dbContext.UserTypes
                    where UT.IsActive && UT.Name != AppConstants.UserType.Employee
                    select new
                    {
                        Value = UT.ID,
                        Text = UT.DisplayName
                    }).Select(x => new ListItem
                    {
                        Value = x.Value.ToString(),
                        Text = x.Text
                    }).ToList();
        }

        public bool UserAlreadyHasEmail(string email)
        {
            bool ret = false;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    ret = dbContext.Users.FirstOrDefault(x => x.Email == email) != null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: UserAlreadyHasEmail Exception Msg", ex.Message);
            }

            return ret;
        }
    }
}
